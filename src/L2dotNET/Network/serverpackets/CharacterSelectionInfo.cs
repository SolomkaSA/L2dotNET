﻿using System.Collections.Generic;
using System.Linq;
using L2dotNET.model.inventory;
using L2dotNET.model.player;

namespace L2dotNET.Network.serverpackets
{
    class CharacterSelectionInfo : GameserverPacket
    {
        private readonly List<L2Player> _players;
        public int CharId = -1;
        private readonly string _account;
        private readonly int _sessionId;

        public CharacterSelectionInfo(string account, List<L2Player> players, int sessionId)
        {
            _players = players;
            _account = account;
            _sessionId = sessionId;
        }

        public override void Write()
        {
            WriteByte(0x13);
            WriteInt(_players.Count);

            //TODO: Make config for that and check valid filters (like char banned, etc) + better checks
            int lastSelectedObjId = 0;

            if (_players.Count > 0)
            {
                if (_players.Count(filter => filter.DeleteTime == 0) == 1)
                    lastSelectedObjId = _players.FirstOrDefault().ObjId;
                else
                    lastSelectedObjId = _players.OrderByDescending(sort => sort.LastAccess).FirstOrDefault(filter => filter.DeleteTime == 0 && filter.LastAccess != null).ObjId;
            }

            foreach (L2Player player in _players)
            {
                WriteString(player.Name);
                WriteInt(player.ObjId);
                WriteString(_account);
                WriteInt(_sessionId);
                WriteInt(player.ClanId);
                WriteInt(0x00); // ??

                WriteInt(player.Sex);
                WriteInt((int)player.BaseClass.ClassId.ClassRace);

                if (player.ActiveClass.ClassId.Id == player.BaseClass.ClassId.Id)
                    WriteInt((int)player.ActiveClass.ClassId.Id);
                else
                    WriteInt((int)player.BaseClass.ClassId.Id);

                WriteInt(0x01); // active ??

                WriteInt(player.X);
                WriteInt(player.Y);
                WriteInt(player.Z);

                WriteDouble(player.CurHp);
                WriteDouble(player.CurMp);

                WriteInt(player.Sp);
                WriteLong(player.Exp);

                WriteInt(player.Level);
                WriteInt(player.Karma);
                WriteInt(player.PkKills);
                WriteInt(player.PvpKills);

                WriteInt(0);
                WriteInt(0);
                WriteInt(0);
                WriteInt(0);
                WriteInt(0);
                WriteInt(0);
                WriteInt(0);

                for (byte id = 0; id < Inventory.PaperdollTotalslots; id++)
                    WriteInt(player.Inventory.Paperdoll[id]?.Template?.ItemId ?? 0);

                for (byte id = 0; id < Inventory.PaperdollTotalslots; id++)
                    WriteInt(player.Inventory.Paperdoll[id]?.Template?.ItemId ?? 0);

                WriteInt(player.HairStyle);
                WriteInt(player.HairColor);

                WriteInt(player.Face);
                WriteDouble(player.CurHp); // hp max TODO
                WriteDouble(player.CurMp); // mp max TODO
                WriteInt(player.RemainingDeleteTime());

                WriteInt((int)player.ActiveClass.ClassId.Id);

                int selection = 0;

                if (CharId != -1)
                    selection = CharId == player.ObjId ? 1 : 0;

                if ((lastSelectedObjId > 0) && (lastSelectedObjId == player.ObjId))
                    selection = 1;

                WriteInt(selection); // auto-select char
                WriteByte(player.GetEnchantValue());
                WriteInt(0x00); // augment
            }
        }
    }
}