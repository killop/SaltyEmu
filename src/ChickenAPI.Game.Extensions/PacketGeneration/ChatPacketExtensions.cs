﻿using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Chats;

namespace ChickenAPI.Game.Extensions.PacketGeneration
{
    public static class ChatPacketExtensions
    {
        public static SayItemPacket GenerateSayItemPacket(this IPlayerEntity player, string prefix, string message, ItemInstanceDto item)
        {
            return new SayItemPacket
            {
                CharacterName = player.Character.Name,
                GlobalPrefix = prefix, // todo i18n
                ItemName = item.Item.Name, // todo i18n
                OratorSlot = 0, // looks like bullshit and useless
                VisualId = player.Id,
                VisualType = player.Type,
                Message = message.Replace(' ', '^'),
                ItemData = item.Item.Type == PocketType.Equipment
                    ? null
                    : new SayItemPacket.SayItemSubPacket
                    {
                        IconId = item.Item.Type == PocketType.Equipment ? (long?)null : item.ItemId,
                    },
                EquipmentInfo = item.Item.Type == PocketType.Equipment ? item.GenerateEInfoPacket() : null
            };
        }
    }
}