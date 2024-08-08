using Magic.DAL;
using Magic.Domain.Entities;
using Magic.Domain.Exceptions;
using Magic.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Service.Implementations
{
    public class GameSessionCharacterService : IGameSessionCharacterService
    {
        protected readonly DataBaseContext _dbContext;

        public GameSessionCharacterService(DataBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GameSessionCharacter> Armor(Guid gameSessionCharacterId, int value)
        {
            var gameSessionCharacter = await GetGameSessionCharacter(gameSessionCharacterId);

            if (!gameSessionCharacter.CurrentShield.HasValue)
            {
                gameSessionCharacter.CurrentShield = 0;
            }

            gameSessionCharacter.CurrentShield += value;
            _dbContext.Update(gameSessionCharacter);
            await _dbContext.SaveChangesAsync();

            return gameSessionCharacter;
        }

        public async Task<GameSessionCharacter> Damage(Guid gameSessionCharacterId, int value)
        {
            var gameSessionCharacter = await GetGameSessionCharacter(gameSessionCharacterId);

            if (gameSessionCharacter.CurrentShield.HasValue && gameSessionCharacter.CurrentShield <= value)
            {
                value -= gameSessionCharacter.CurrentShield.Value;
                gameSessionCharacter.CurrentShield = null;
            }

            if (gameSessionCharacter.CurrentHP <= value)
            {
                gameSessionCharacter.CurrentHP = 0;
            }
            else
            {
                gameSessionCharacter.CurrentHP -= value;
            }

            _dbContext.Update(gameSessionCharacter);
            await _dbContext.SaveChangesAsync();

            return gameSessionCharacter;
        }

        public async Task<int> GetRollModificator(Guid gameSessionCharacterId, int characterCharacteristicId)
        {
            var gameSessionCharacter = await GetGameSessionCharacter(gameSessionCharacterId);
            var characterCharacteristicValue = gameSessionCharacter.Characteristics[characterCharacteristicId];

            var result = characterCharacteristicValue switch
            {
                < 2 => -5,
                < 4 and > 1 => -4,
                < 6 and > 3 => -3,
                < 8 and > 5 => -2,
                < 10 and > 7 => -1,
                < 12 and > 9 => 0,
                < 14 and > 11 => 1,
                < 16 and > 13 => 2,
                < 18 and > 15 => 3,
                < 20 and > 17 => 4,
                < 22 and > 19 => 5,
                < 24 and > 21 => 6,
                < 26 and > 23 => 7,
                < 28 and > 25 => 8,
                < 30 and > 27 => 9,
                >= 30 => 10
            };
            return result;
        }

        public async Task<GameSessionCharacter> Heal(Guid gameSessionCharacterId, int value)
        {
            var gameSessionCharacter = await GetGameSessionCharacter(gameSessionCharacterId);

            gameSessionCharacter.CurrentHP += value;
            _dbContext.Update(gameSessionCharacter);
            await _dbContext.SaveChangesAsync();

            return gameSessionCharacter;
        }

        public async Task<GameSessionCharacter> SetPosition(Guid gameSessionCharacterId, int? x, int? y)
        {
            var gameSessionCharacter = await GetGameSessionCharacter(gameSessionCharacterId);
            gameSessionCharacter.PositionX = x;
            gameSessionCharacter.PositionY = y;

            _dbContext.Update(gameSessionCharacter);
            await _dbContext.SaveChangesAsync();

            return gameSessionCharacter;
        }

        private async Task<GameSessionCharacter> GetGameSessionCharacter(Guid gameSessionCharacterId)
        {
            var gameSessionCharacter = await _dbContext.GameSessionCharacters
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == gameSessionCharacterId);

            if (gameSessionCharacter == null)
            {
                throw new ExceptionWithApplicationCode("Игровой персонаж не найден",
                    Domain.Enums.ExceptionApplicationCodeEnum.GameSessionCharacterNotFound);
            }

            return gameSessionCharacter;
        }
    }
}