using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace winwolf.util
{
    public class Client
    {
        public Socket socket = null;
        public byte[] buffer = new byte[0];
        public byte[] tempbuffer = new byte[1024];
        public int length = 0;
        public int capacity = 0;
        private bool closed = false;

        /// <summary>
        /// 客户端已关闭
        /// </summary>
        public bool Closed
        {
            get { return this.closed; }
            set { this.closed = value; }
        }

        /// <summary>
        /// 将字节流写入缓冲区
        /// </summary>
        /// <param name="bufferEx">要写入缓冲区的字节流</param>
        /// <param name="offset">写入字节流的开始位置</param>
        /// <param name="count">写入字节大小</param>
        public void Write(byte[] buf, int offset, int count)
        {
            if (count > buf.Length - offset)
            {
                count = buf.Length - offset;
            }
            EnlargeCapacity(length + count);//再写入之前，判断容量大小

            Buffer.BlockCopy(buf, offset, buffer, length, count);
            length += count;
        }

        /// <summary>
        /// 确保容量足够大
        /// </summary>
        /// <param name="count"></param>
        public void EnlargeCapacity(int count)
        {
            if (count <= capacity)
            {
                return;
            }
            if (count < 2 * capacity)
            {
                count = 2 * capacity;
            }

            byte[] buf = new byte[count];
            capacity = count;
            Buffer.BlockCopy(buffer, 0, buf, 0, length);
            buffer = buf;
        }
    }
}
