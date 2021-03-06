﻿using L2dotNET.model.player;

namespace L2dotNET.Network.clientpackets
{
    class RequestSkillList : PacketBase
    {
        private readonly GameClient _client;

        public RequestSkillList(Packet packet, GameClient client)
        {
            _client = client;
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;
            player.UpdateSkillList();
        }
    }
}
