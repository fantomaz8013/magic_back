using Magic.Common.Models.Request;
using Magic.Common.Models.Response;
using Magic.DAL;
using Magic.Domain.Entities;
using Magic.Domain.Exceptions;
using Magic.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Magic.Utils;
using Magic.Domain.Enums;

namespace Magic.Service.Implementations;

public class MapService : IMapService
{
    protected readonly DataBaseContext _dbContext;
    protected readonly ITilePropertyService _tilePropertyService;
    protected readonly IGameSessionCharacterService _gameSessionCharacterService;
    protected readonly IGameSessionCharacterTurnInfoService _gameSessionCharacterTurnInfoService;

    public MapService(DataBaseContext dbContext, ITilePropertyService tilePropertyService, IGameSessionCharacterService gameSessionCharacterService, IGameSessionCharacterTurnInfoService gameSessionCharacterTurnInfoService)
    {
        _dbContext = dbContext;
        _tilePropertyService = tilePropertyService;
        _gameSessionCharacterService = gameSessionCharacterService;
        _gameSessionCharacterTurnInfoService = gameSessionCharacterTurnInfoService;
    }
    public async Task<List<MapResponse>> GetMaps()
    {
        var maps = await _dbContext.Maps
            .Select(x => new MapResponse(x))
            .ToListAsync();
        return maps;
    }

    /// <summary>
    /// Расчитать, можно ли пройти по указаному маршруту в карте. path = (индекс пути, LocationRequest) пример: [<0,Location(2,3)>, <1,Location(3,4)>, <2,Location(3,5)>]
    /// </summary>
    /// <param name="path"> (индекс пути, LocationRequest) пример: [<0,Location(2,3)>, <1,Location(3,4)>, <2,Location(3,5)>]</param>
    /// <param name="mapId">Идентификатор карты</param>
    /// <param name="gameSessionCharacterId">Идентификатор игрового персонажа</param>
    /// <returns></returns>
    public async Task<PathCalculationResponse> PathCalculation(Dictionary<int, LocationRequest> path, Guid mapId, Guid gameSessionCharacterId)
    {
        //Результат просчета пути
        var pathCalculationResponse = new PathCalculationResponse();
        if (path.Count == 0)
        {
            throw new ExceptionWithApplicationCode("Проверяемый маршрут пустой",
               ExceptionApplicationCodeEnum.PathIsEmpty);
        }
        var map = await GetMapById(mapId);
        var gameSessionCharacter = await _gameSessionCharacterService.GetGameSessionCharacter(gameSessionCharacterId);
        LocationRequest? prevLocation = null;

        if (!gameSessionCharacter.PositionX.HasValue || !gameSessionCharacter.PositionY.HasValue)
        {
            throw new ExceptionWithApplicationCode("Персонаж не размещен на игровой карте",
               ExceptionApplicationCodeEnum.CharacterNotInMap);
        }

        var characterTurnInfo = await _gameSessionCharacterTurnInfoService
            .GetCharacterTurnInfo(gameSessionCharacterId);

        if (characterTurnInfo.SkipStepCount > 0)
        {
            pathCalculationResponse.Result = false;
            pathCalculationResponse.Message = $"Вы не можете совершить это действие. Количество ходов нейтрализации: {characterTurnInfo.SkipStepCount}";
        }

        //Количество очков передвижения персонажа
        var characterSpeed = characterTurnInfo.LeftStep;
        //Количество текущего здоровья с учетом брони
        var characterHealth = gameSessionCharacter.CurrentHP + (gameSessionCharacter.CurrentShield ?? 0);
        //Проверка, хватает ли количества ходов для прохода всего пути
        if (characterSpeed < path.Count)
        {
            pathCalculationResponse.Result = false;
            pathCalculationResponse.Message = "Не хватает очков передвижения";
            return pathCalculationResponse;
        }

        var firstPoint = path.First();

        //Проверка находится ли персонаж рядом с первой точкой пути
        if (!CalculatePathUtil.IsNeighboringPoint(gameSessionCharacter.PositionX.Value, gameSessionCharacter.PositionY.Value, firstPoint.Value.X, firstPoint.Value.Y))
        {
            pathCalculationResponse.Result = false;
            pathCalculationResponse.Message = $"Персонаж находится слишком далеко от начальной точкой пути. " +
                $"Начальная точка: ({gameSessionCharacter.PositionX.Value},{gameSessionCharacter.PositionY.Value}) " +
                $"следующая точка: ({firstPoint.Value.X},{firstPoint.Value.Y})";
            return pathCalculationResponse;
        }


        //Проверяем каждый шаг в пути
        foreach (var item in path)
        {
            var x = item.Value.X;
            var y = item.Value.Y;
            //Проверка, находится ли точка по соседству с прошлой точкой
            if (prevLocation != null && !CalculatePathUtil.IsNeighboringPoint(item.Value.X, item.Value.Y, prevLocation.X, prevLocation.Y))
            {
                pathCalculationResponse.Result = false;
                pathCalculationResponse.Message = $"Между точками ({item.Value.X},{item.Value.Y}) и ({prevLocation.X},{prevLocation.Y}) слишком большое растояние!";
                return pathCalculationResponse;
            }

            var tilePropertyId = map.Tiles.ElementAt(x).ElementAt(y);
            var tileProperty = await _tilePropertyService.GetTileProperty(tilePropertyId);

            //Проверка, можно ли пройти по тайлу
            if (tileProperty.CollisionType != TilePropertyCollisionTypeEnum.None)
            {
                pathCalculationResponse.Result = false;
                pathCalculationResponse.Message = $"По тайлу({x},{y}) невозможно пройти";
                return pathCalculationResponse;
            }

            //Проверка, будет ли штраф за проход по тайлу
            if (tileProperty.PenaltyType != TilePropertyPenaltyTypeEnum.None && tileProperty.PenaltyValue.HasValue)
            {
                pathCalculationResponse.Penalties.Add(new PenaltyResponse
                {
                    PenaltyType = tileProperty.PenaltyType,
                    Value = tileProperty.PenaltyValue.Value
                });
            }

            //Записываем позицию перед следующей итерацией
            prevLocation = item.Value;
            characterSpeed--;
        }

        var countPenaltySpeed = pathCalculationResponse.Penalties
            .Where(x => x.PenaltyType == TilePropertyPenaltyTypeEnum.PenaltySpeed)
            .Sum(x => x.Value);

        var countPenaltyHealth = pathCalculationResponse.Penalties
            .Where(x => x.PenaltyType == TilePropertyPenaltyTypeEnum.PenaltyHealth)
            .Sum(x => x.Value);

        //Проверка, хватает ли очков передвижения с учетом наложеных штрафов. Тут идет в учет уже оставшееся количество очков передвижения после всего пути
        if (characterSpeed - countPenaltySpeed < 0)
        {
            pathCalculationResponse.Result = false;
            pathCalculationResponse.Message = $"Не хватает очков передвижения после наложеных штрафов.";
            return pathCalculationResponse;
        }

        //Проверка, хватает ли очков здоровья ( с учетом брони ) после всех штрафов
        if (characterHealth - countPenaltyHealth <= 0)
        {
            pathCalculationResponse.Result = false;
            pathCalculationResponse.Message = $"Не хватает очков здоровья после наложеных штрафов.";
            return pathCalculationResponse;
        }

        //Если забыли сделать return до этого, то возвращаем, чтобы случайно не записать новую локацию персонажа
        if (!pathCalculationResponse.Result)
        {
            return pathCalculationResponse;
        }

        //Записываем новую локацию персонажа
        pathCalculationResponse.NewCharacterPosition = new PositionResponse
        {
            X = prevLocation!.X, 
            Y = prevLocation!.Y
        };

        return pathCalculationResponse;
    }

    private async Task<Map> GetMapById(Guid mapId)
    {
        var map = await _dbContext.Maps
            .FirstOrDefaultAsync( x => x.Id == mapId);
        if (map == null)
        {
            throw new ExceptionWithApplicationCode("Карта не найдена",
               ExceptionApplicationCodeEnum.MapNotExist);
        }
        return map;
    } 
}