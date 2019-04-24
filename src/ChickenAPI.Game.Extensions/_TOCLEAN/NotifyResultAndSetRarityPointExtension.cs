﻿using System.Threading.Tasks;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using ChickenAPI.Packets.Enumerations;
using EquipmentType = ChickenAPI.Enums.Game.Items.EquipmentType;

namespace ChickenAPI.Game.Inventory.ItemUpgrade.Extension
{
    public static class NotifyResultAndSetRarityPointExtension
    {
        public static async Task RarifyAsync(this RarifyEvent e, IPlayerEntity player, sbyte rarity)
        {
            if (e.Mode != RarifyMode.Drop)
            {
                await player.NotifyRarifyResult(rarity);
            }

            e.Item.Rarity = rarity;
            /* GenerateHeroicShell(protection);*/
            e.Item.SetRarityPoint();
        }

        public static async Task FailAsync(this RarifyEvent e, IPlayerEntity player)
        {
            if (e.Mode != RarifyMode.Drop)
            {
                switch (e.Protection)
                {
                    case RarifyProtection.BlueAmulet:
                    case RarifyProtection.RedAmulet:
                    case RarifyProtection.HeroicAmulet:
                    case RarifyProtection.RandomHeroicAmulet:
                        ItemInstanceDto amulets = player.Inventory.GetItemFromSlotAndType((short)EquipmentType.Amulet, PocketType.Wear);
                        if (amulets == null)
                        {
                            return;
                        }

                        /* amulet.DurabilityPoint -= 1;
                         if (amulet.DurabilityPoint <= 0)
                         {
                             session.Character.DeleteItemByItemInstanceId(amulet.Id);
                             await session.SendPacketAsync($"info {Language.Instance.GetMessageFromKey("AMULET_DESTROYED")}");
                             await session.SendPacketAsync(session.Character.GenerateEquipment());
                         }*/
                        await player.SendTopscreenMessage("AMULET_FAIL_SAVED", MessageType.Whisper);
                        await player.SendChatMessageAsync("AMULET_FAIL_SAVED", SayColorType.Purple);
                        return;

                    case RarifyProtection.None:
                        /* session.Character.DeleteItemByItemInstanceId(Id);*/
                        //player.EmitEvent(new InventoryDestroyItemEvent { ItemInstance = e.Item });
                        await player.SendTopscreenMessage("RARIFY_FAILED", MessageType.Whisper);
                        await player.SendChatMessageAsync("RARIFY_FAILED", SayColorType.Purple);

                        return;
                }

                await player.SendTopscreenMessage("RARIFY_FAILED_ITEM_SAVED", MessageType.Whisper);
                await player.SendChatMessageAsync("RARIFY_FAILED_ITEM_SAVED", SayColorType.Purple);
                await player.BroadcastAsync(player.GenerateEffectPacket(3004));
            }
        }
    }
}