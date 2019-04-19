using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace KinectStreams
{
    class KinectCSVManager
    {
        int _current = 0;

        bool _hasEnumeratedJoints = false;

        public bool IsRecording { get; protected set; }

        public string Folder { get; protected set; }

        public string Result { get; protected set; }

        public void Start()
        {
            IsRecording = true;
            Folder = DateTime.Now.ToString("yyy_MM_dd_HH_mm_ss");

            Directory.CreateDirectory(Folder);
        }

        public void Update(Body body)
        {
            var joints = body.Joints;
            var orientations = body.JointOrientations;
            if (!IsRecording) return;
            if (body == null || !body.IsTracked) return;

            string path = Path.Combine(Folder, _current.ToString() + ".line");

            using (StreamWriter writer = new StreamWriter(path))
            {
                StringBuilder line = new StringBuilder();

                if (!_hasEnumeratedJoints)
                {
                    foreach (var joint in body.Joints.Values)
                    {
                    //line.Append(string.Format("{0};;;", joint.JointType.ToString()));
                      line.Append(string.Format("{0}\t\t\t\t\t\t\t", joint.JointType.ToString()));
                    }
                    line.AppendLine();

                    foreach (var joint in body.Joints.Values)
                    {
                        //line.Append("X;Y;Z;");
                        line.Append("Position.X;Position.Y;Position.Z;Orientation.X;Orientation.Y;Orientation.Z;Orientation.W;");
                    }
                    line.AppendLine();

                    _hasEnumeratedJoints = true;
                }

                //foreach (var joint in body.Joints.Values)
                foreach ( var jointType in joints.Keys )
                {
                    var position = joints[jointType].Position;
                    var orientation = orientations[jointType].Orientation;
                    //line.Append(string.Format("{0};{1};{2};", joint.Position.X, joint.Position.Y, joint.Position.Z));
                    line.Append(string.Format("{0};{1};{2};{3};{4};{5};{6};",position.X, position.Y, position.Z,orientation.X,orientation.Y,orientation.Z,orientation.W));
                    //line.Append((int)jointType);
                    //line.AppendLine(); 
                }

                line.AppendLine();


                //determine if joint type numbers match with the one on medium.com (Add another field to the positions list called joint type)
                //1. Hands up 
                JointType left_wrist, right_wrist, spine_mid, spine_base;
                left_wrist = (JointType)6;
                right_wrist = (JointType)10;
                spine_mid = (JointType)1;
                spine_base = (JointType)0;
                var position1 = joints[left_wrist].Position;
                var position2 = joints[right_wrist].Position;
                var orientation1 = orientations[spine_mid].Orientation;
                var orientation2 = orientations[spine_base].Orientation;
                if (position1.Y > 0 && position2.Y > 0)
                {

                    Console.WriteLine("BOTH HANDS ARE UP.");
                    
                }
                else if (position1.Y > 0 && position2.Y < 0)
                {

                    Console.WriteLine("LEFT HAND UP.");
                }

                else if (position1.Y < 0 && position2.Y > 0)
                {

                    Console.WriteLine("RIGHT HAND UP.");
                }

                //2.Hands Crossed
                else if(position1.X > 0 && position2.X < 0)
                {
                    Console.WriteLine("HANDS CROSSED.");
                }

                //3. orientation
                else if(orientation1.X < -0.02 && orientation2.X < -0.02)
                {
                    Console.WriteLine("FACING RIGHT.");
                }
                

                writer.Write(line);

                _current++;
            }
        }

        public void Stop()
        {
            IsRecording = false;
            _hasEnumeratedJoints = false;

            Result = DateTime.Now.ToString("yyy_MM_dd_HH_mm_ss") + ".csv";

            using (StreamWriter writer = new StreamWriter(Result))
            {
                for (int index = 0; index < _current; index++)
                {
                    string path = Path.Combine(Folder, index.ToString() + ".line");

                    if (File.Exists(path))
                    {
                        string line = string.Empty;

                        using (StreamReader reader = new StreamReader(path))
                        {
                            line = reader.ReadToEnd();
                        }

                        writer.WriteLine(line);
                    }
                }
            }

            Directory.Delete(Folder, true);
        }
    }
}
