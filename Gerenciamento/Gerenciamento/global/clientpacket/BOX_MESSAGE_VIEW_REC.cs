using System;
using System.Collections.Generic;
using Core;
using Core.server;

namespace Game.global.clientpacket
{
	// Token: 0x0200011C RID: 284
	public class BOX_MESSAGE_VIEW_REC : ReceiveGamePacket
	{
		// Token: 0x060002D2 RID: 722 RVA: 0x00013458 File Offset: 0x00011658
		public BOX_MESSAGE_VIEW_REC(GameClient client, byte[] data)
		{
			base.makeme(client, data);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00013474 File Offset: 0x00011674
		public override void read()
		{
			this.msgsCount = (int)base.readC();
			for (int i = 0; i < this.msgsCount; i++)
			{
				this.messages.Add(base.readD());
			}
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x000134B0 File Offset: 0x000116B0
		public override void run()
		{
			try
			{
				if (this._client != null && this._client._player != null && this.msgsCount != 0)
				{
					ComDiv.updateDB("player_messages", "object_id", this.messages.ToArray(), "owner_id", this._client.player_id, new string[]
					{
						"expire",
						"state"
					}, new object[]
					{
						long.Parse(DateTime.Now.AddDays(7.0).ToString("yyMMddHHmm")),
						0
					});
				}
			}
			catch (Exception ex)
			{
				Logger.info("[BOX_MESSAGE_VIEW_REC] " + ex.ToString());
			}
		}

		// Token: 0x040002BE RID: 702
		private int msgsCount;

		// Token: 0x040002BF RID: 703
		private List<int> messages = new List<int>();
	}
}
