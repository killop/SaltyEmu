﻿using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Core.Data.TransferObjects;
using ChickenAPI.Enums.Game.Portals;

namespace SaltyEmu.DatabasePlugin.Models.Map
{
    [Table("map_portals")]
    public class MapPortalModel : IMappedDto
    {
        public PortalType Type { get; set; }

        public long DestinationMapId { get; set; }

        public short DestinationX { get; set; }

        public short DestinationY { get; set; }

        public bool IsDisabled { get; set; }

        public MapModel SourceMap { get; set; }


        [ForeignKey(nameof(SourceMapId))]
        public long SourceMapId { get; set; }

        public short SourceX { get; set; }

        public short SourceY { get; set; }
        public long Id { get; set; }
    }
}