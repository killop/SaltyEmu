﻿using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Data.AccessLayer.Skill;
using ChickenAPI.Game.ECS.Components;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Features.Skills
{
    public class SkillComponent : IComponent
    {
        public SkillComponent(IEntity entity)
        {
            Entity = entity;
            CharacterSkills = new Dictionary<Guid, CharacterSkillDto>();
            Skills = new Dictionary<long, SkillDto>();
            CooldownsBySkillId = new List<(DateTime, long)>();

            if (!(entity is IPlayerEntity player))
            {
                return;
            }

            int tmp = 200 + 20 * (byte)player.Character.Class;
            Skills.Add(tmp, SkillService.GetById(tmp));
            Skills.Add(tmp + 1, SkillService.GetById(tmp + 1));

            if (player.Character.Class == CharacterClassType.Adventurer)
            {
                Skills.Add(tmp + 9, SkillService.GetById(tmp + 9));
            }
        }

        public SkillComponent(IEntity entity, IEnumerable<CharacterSkillDto> skills) : this(entity)
        {
            foreach (CharacterSkillDto characterSkill in skills)
            {
                CharacterSkills.Add(characterSkill.Id, characterSkill);
                SkillDto skill = characterSkill.Skill;
                if (!Skills.ContainsKey(skill.Id))
                {
                    Skills.Add(skill.Id, skill);
                }
            }
        }

        public Dictionary<Guid, CharacterSkillDto> CharacterSkills { get; }

        private static readonly ISkillService SkillService = new Lazy<ISkillService>(() => ChickenContainer.Instance.Resolve<ISkillService>()).Value;

        public Dictionary<long, SkillDto> Skills { get; }

        public List<(DateTime, long)> CooldownsBySkillId { get; }

        public IEntity Entity { get; }
    }
}