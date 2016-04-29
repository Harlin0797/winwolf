using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using winwolf.util;

namespace winwolf.events
{
    public class MsgChangedEventArgs:EventArgs
    {
        private Client _client;
        private byte[] _content;
        private int _length;

        public MsgChangedEventArgs(byte[] content,int length,Client client)
        {
            this._content = content;
            this._length = length;
            this._client = client;
        }

        public Client Client
        {
            get { return this._client; }
            set { this._client = value; }
        }

        /// <summary>
        /// 接收到的数据内容
        /// </summary>
        public byte[] Content
        {
            get { return this._content; }
            set { this._content = value; }
        }

        /// <summary>
        /// 接收到的数据长度
        /// </summary>
        public int Length
        {
            get { return this._length; }
            set { this._length = value; }
        }
    }
}
