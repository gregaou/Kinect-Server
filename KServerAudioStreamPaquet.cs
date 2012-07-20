using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectServer
{
    class KServerAudioStreamPaquet : KServerStreamPaquet
    {
        private const byte StreamId = 204;
        private int idSensor;
        private byte[] audioData;

        public KServerAudioStreamPaquet(byte[] _audioData, int _idSensor)
        {
            audioData = _audioData;
            idSensor = _idSensor;

            setBodySize((uint)(audioData.Length+1));

            byte[] size = BitConverter.GetBytes((uint)(audioData.Length + 1));

            Array.Reverse(size);
            Buffer.BlockCopy(size, 0, data, 0, size.Length);

            /* Builds the paquet */
            build();

        }

        protected override byte id()
        {
            return StreamId;
        }

        protected override void buildBody()
        {
            data[headerSize()] = (byte)(idSensor);

            /* Copies the data in the paquet buffer */
            Buffer.BlockCopy(audioData, 0, data, (int)headerSize() + 1, audioData.Length);

            Console.WriteLine("Audio Stream Paquet : taille = " + audioData.Length + " | idSensor = " + idSensor);

        }
    }   
}
