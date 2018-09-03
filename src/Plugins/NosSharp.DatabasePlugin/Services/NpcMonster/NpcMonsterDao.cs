﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ChickenAPI.Game.Data.AccessLayer.NpcMonster;
using ChickenAPI.Game.Data.TransferObjects.NpcMonster;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Models.NpcMonster;
using SaltyEmu.DatabasePlugin.Services.Base;

namespace SaltyEmu.DatabasePlugin.Services.NpcMonster
{
    public class NpcMonsterDao : MappedRepositoryBase<NpcMonsterDto, NpcMonsterModel>, INpcMonsterService
    {
        private readonly Dictionary<long, NpcMonsterDto> _monsters = new Dictionary<long, NpcMonsterDto>();

        public NpcMonsterDao(NosSharpContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override NpcMonsterDto GetById(long id)
        {
            if (_monsters.TryGetValue(id, out NpcMonsterDto value))
            {
                return value;
            }

            value = base.GetById(id);
            _monsters[id] = value;

            return value;
        }

        public override async Task<NpcMonsterDto> GetByIdAsync(long id)
        {
            if (_monsters.TryGetValue(id, out NpcMonsterDto value))
            {
                return value;
            }

            value = await base.GetByIdAsync(id);
            _monsters[id] = value;

            return value;
        }
    }
}