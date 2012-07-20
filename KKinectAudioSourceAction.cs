using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using Microsoft.Kinect;
using System.Threading;

namespace KinectServer
{
    class KKinectAudioSourceAction : KAction
    {
        public KKinectAudioSourceAction(NetworkStream arg)
            : base(arg)
        {
            
        }

        private KinectSensor sensor;

        private void getKinectSensor(int id){
            try
            {
                sensor = sensors[id];
            }
            catch (Exception e)
            {
                throw new KActionException(KError.ArgumentOutOfRange);
            }
        }

        public byte getAutomaticGainControlEnabled(string[] args){

            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += (sensor.AudioSource.AutomaticGainControlEnabled)?1:0;

            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            return KSuccess.QueryOk;
        }

        public byte setAutomaticGainControlEnabled(string[] args)
        {

            try
            {
                verifArgs(2, args);
                getKinectSensor(int.Parse(args[0]));
                sensor.AudioSource.AutomaticGainControlEnabled = bool.Parse(args[1]);

            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            return KSuccess.QueryOk;
        }


        public byte getBeamAngle(string[] args)
        {

            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += sensor.AudioSource.BeamAngle;

            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            return KSuccess.QueryOk;
        }


        public byte getBeamAngleMode(string[] args)
        {

            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += (byte)sensor.AudioSource.BeamAngleMode;

            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            return KSuccess.QueryOk;
        }

        public byte setBeamAngleMode(string[] args)
        {

            try
            {
                verifArgs(2, args);
                getKinectSensor(int.Parse(args[0]));
                sensor.AudioSource.BeamAngleMode = (BeamAngleMode)int.Parse(args[1]);

            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            return KSuccess.QueryOk;
        }


        public byte getEchoCancellationMode(string[] args)
        {

            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += sensor.AudioSource.EchoCancellationMode;

            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            return KSuccess.QueryOk;
        }

        public byte setEchoCancellationMode(string[] args)
        {

            try
            {
                verifArgs(2, args);
                getKinectSensor(int.Parse(args[0]));
                sensor.AudioSource.EchoCancellationMode = (EchoCancellationMode)int.Parse(args[1]);

            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            return KSuccess.QueryOk;
        }


        public byte getEchoCancellationSpeakerIndex(string[] args)
        {

            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += sensor.AudioSource.EchoCancellationMode;

            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            return KSuccess.QueryOk;
        }

        public byte setEchoCancellationSpeakerIndex(string[] args)
        {

            try
            {
                verifArgs(2, args);
                getKinectSensor(int.Parse(args[0]));
                sensor.AudioSource.EchoCancellationSpeakerIndex = int.Parse(args[1]);

            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            return KSuccess.QueryOk;
        }



        public byte getManualBeamAngle(string[] args)
        {

            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += sensor.AudioSource.ManualBeamAngle;

            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            return KSuccess.QueryOk;
        }

        public byte setManualBeamAngle(string[] args)
        {

            try
            {
                verifArgs(2, args);
                getKinectSensor(int.Parse(args[0]));
                sensor.AudioSource.ManualBeamAngle = int.Parse(args[1]);

            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            return KSuccess.QueryOk;
        }


        public byte getMaxBeamAngle(string[] args)
        {

            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += KinectAudioSource.MaxBeamAngle;

            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            return KSuccess.QueryOk;
        }

        public byte getMaxSoundSourceAngle(string[] args)
        {

            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += KinectAudioSource.MaxSoundSourceAngle;

            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            return KSuccess.QueryOk;
        }

        public byte getMinBeamAngle(string[] args)
        {

            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += KinectAudioSource.MinBeamAngle;

            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            return KSuccess.QueryOk;
        }

        public byte getMinSoundSourceAngle(string[] args)
        {

            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += KinectAudioSource.MinSoundSourceAngle;

            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            return KSuccess.QueryOk;
        }


        public byte getNoiseSuppression(string[] args)
        {

            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += (sensor.AudioSource.NoiseSuppression)?1:0;

            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            return KSuccess.QueryOk;
        }

        public byte setNoiseSuppression(string[] args)
        {

            try
            {
                verifArgs(2, args);
                getKinectSensor(int.Parse(args[0]));
                sensor.AudioSource.NoiseSuppression = (args[1] == "1")?true:false;

            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            return KSuccess.QueryOk;
        }


        public byte getSoundSourceAngle(string[] args)
        {

            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += sensor.AudioSource.SoundSourceAngle;

            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            return KSuccess.QueryOk;
        }


        public byte getSoundSourceAngleConfidence(string[] args)
        {

            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                rData += sensor.AudioSource.SoundSourceAngleConfidence;

            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            return KSuccess.QueryOk;
        }


        public byte stop(string[] args)
        {

            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                sensor.AudioSource.Stop();
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            rData = "Succefull !";
            return KSuccess.QueryOk;
        }

        public byte BeamAngleChanged(string[] args)
        {

            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                sensor.AudioSource.BeamAngleChanged += KinectAudioSourceBeamAngleChanged;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            rData = "Succefull !";
            return KSuccess.QueryOk;
        }

        public void KinectAudioSourceBeamAngleChanged(Object Sender, BeamAngleChangedEventArgs e)
        {


            try
            {
                string message = new string('0',0);

                message += getIdSensor(sensor) + "||";
                message += e.Angle;

                KServerPaquet sp = new KServerMessagePaquet(205, message.Replace(',', '.'));
                sp.send(ns);
            }
            catch (Exception exc)
            {
                System.Console.WriteLine("event AudioSourceBeamAngleChanged disconnected : " + exc.Message);
                sensor.AudioSource.BeamAngleChanged -= KinectAudioSourceBeamAngleChanged;
            }
        }

        public byte SoundSourceAngleChanged(string[] args)
        {

            try
            {
                verifArgs(1, args);
                getKinectSensor(int.Parse(args[0]));
                sensor.AudioSource.SoundSourceAngleChanged += KinectAudioSourceSoundSourceAngleChanged;
            }
            catch (KActionException e)
            {
                rData = e.Message;
                return e.exceptionNumber;
            }

            rData = "Succefull !";
            return KSuccess.QueryOk;
        }

        public void KinectAudioSourceSoundSourceAngleChanged(Object Sender, SoundSourceAngleChangedEventArgs e)
        {
            try
            {
                string message = new string('0', 0);

                message += getIdSensor(sensor) + "||";
                message += e.Angle + "||" + e.ConfidenceLevel;

                KServerPaquet sp = new KServerMessagePaquet(206, message.Replace(',', '.'));
                sp.send(ns);
            }
            catch (Exception exc)
            {
                System.Console.WriteLine("event AudioSourceBeamAngleChanged disconnected : " + exc.Message);
                sensor.AudioSource.SoundSourceAngleChanged -= KinectAudioSourceSoundSourceAngleChanged;
            }
        }



        private int getIdSensor(KinectSensor s)
        {
            for (int i = 0; i < sensors.Count; i++)
            {
                if (s.Equals(sensors[i]))
                    return i;
            }
            return -1;
        }


    }
}
