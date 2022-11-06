using System;
using Core;
using Core.managers;
using Core.models.account.players;
using Core.server;
using Game.data.model;
using Game.global.serverpacket;

namespace Game.global.clientpacket
{
	// Token: 0x0200011F RID: 287
	public class BASE_QUEST_DELETE_CARD_SET_REC : ReceiveGamePacket
	{
		// Token: 0x060002DB RID: 731 RVA: 0x0000E032 File Offset: 0x0000C232
		public BASE_QUEST_DELETE_CARD_SET_REC(GameClient client, byte[] data)
		{
			base.makeme(client, data);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00013758 File Offset: 0x00011958
		public override void read()
		{
			this.missionIdx = (int)base.readC();
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00013768 File Offset: 0x00011968
		public override void run()
		{
			try
			{
				Account player = this._client._player;
				if (player != null)
				{
					PlayerMissions mission = player._mission;
					bool flag = false;
					if (this.missionIdx >= 3 || (this.missionIdx == 0 && mission.mission1 == 0) || (this.missionIdx == 1 && mission.mission2 == 0) || (this.missionIdx == 2 && mission.mission3 == 0))
					{
						flag = true;
					}
					if (!flag && PlayerManager.updateMissionId(player.player_id, 0, this.missionIdx) && ComDiv.updateDB("player_missions", "owner_id", player.player_id, new string[]
					{
						"card" + (this.missionIdx + 1).ToString(),
						"mission" + (this.missionIdx + 1).ToString()
					}, new object[]
					{
						0,
						new byte[0]
					}))
					{
						if (this.missionIdx == 0)
						{
							mission.mission1 = 0;
							mission.card1 = 0;
							mission.list1 = new byte[40];
						}
						else if (this.missionIdx == 1)
						{
							mission.mission2 = 0;
							mission.card2 = 0;
							mission.list2 = new byte[40];
						}
						else if (this.missionIdx == 2)
						{
							mission.mission3 = 0;
							mission.card3 = 0;
							mission.list3 = new byte[40];
						}
					}
					else
					{
						this.erro = 2147487824U;
					}
					this._client.SendPacket(new BASE_QUEST_DELETE_CARD_SET_PAK(this.erro, player));
				}
			}
			catch (Exception ex)
			{
				Logger.info("BASE_QUEST_DELETE_CARD_SET_REC: " + ex.ToString());
			}
		}

		// Token: 0x040002C3 RID: 707
		private uint erro;

		// Token: 0x040002C4 RID: 708
		private int missionIdx;
	}
}
