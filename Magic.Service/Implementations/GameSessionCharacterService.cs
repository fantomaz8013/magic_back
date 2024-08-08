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
            int result = 0;

            if (characterCharacteristicValue < 2)
            {
                result = -5;
            }
            if (characterCharacteristicValue < 4 && characterCharacteristicValue > 1)
            {
                result = -4;
            }
            if (characterCharacteristicValue < 6 && characterCharacteristicValue > 3)
            {
                result = -3;
            }
            if (characterCharacteristicValue < 8 && characterCharacteristicValue > 5)
            {
                result = -2;
            }
            if (characterCharacteristicValue < 10 && characterCharacteristicValue > 7)
            {
                result = -1;
            }
            if (characterCharacteristicValue < 12 && characterCharacteristicValue > 9)
            {
                result = 0;
            }
            if (characterCharacteristicValue < 14 && characterCharacteristicValue > 11)
            {
                result = 1;
            }
            if (characterCharacteristicValue < 16 && characterCharacteristicValue > 13)
            {
                result = 2;
            }
            if (characterCharacteristicValue < 18 && characterCharacteristicValue > 15)
            {
                result = 3;
            }
            if (characterCharacteristicValue < 20 && characterCharacteristicValue > 17)
            {
                result = 4;
            }
            if (characterCharacteristicValue < 22 && characterCharacteristicValue > 19)
            {
                result = 5;
            }
            if (characterCharacteristicValue < 24 && characterCharacteristicValue > 21)
            {
                result = 6;
            }
            if (characterCharacteristicValue < 26 && characterCharacteristicValue > 23)
            {
                result = 7;
            }
            if (characterCharacteristicValue < 28 && characterCharacteristicValue > 25)
            {
                result = 8;
            }
            if (characterCharacteristicValue < 30 && characterCharacteristicValue > 27)
            {
                result = 9;
            }
            if (characterCharacteristicValue >= 30 )
            {
                result = 10;
            }
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

        public async Task<GameSessionCharacter> SetPosition(Guid gameSessionCharacterId, int? x , int? y )
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
