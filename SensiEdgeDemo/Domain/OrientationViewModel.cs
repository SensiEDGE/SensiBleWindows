using SensiEdge.Data;
using SensiEdge.Device;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace SensiEdgeDemo.Domain
{
    public class OrientationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => GetText((Orientation)value);

        private string GetText(Orientation value)
        {
            if (value is null) return "";

            float X = value.Quat0.X / (float)10000.0;
            float Y = value.Quat0.Y / (float)10000.0;
            float Z = value.Quat0.Z / (float)10000.0;
            float tmp = (float)(1 - (Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2)));
            float W = (float)Math.Sqrt(tmp);

            return $"Quaternion: [X]{X:F3}, [Y]{Y:F3)}, [Z]{Z:F3}, [W]{W:F3}";
        }

        private double GetAngle(short value)
        {
            return (value > 0) ? 0.01 * value : -0.01 * value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    }
    public class TransformConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => GetTransform((Orientation)value);

        private Matrix3D ViewMatrix
        {
            get
            {
                return new Matrix3D(
                        0, 1, 0, 0,
                        0, 0, 1, 0,
                        1, 0, 0, 0,
                        0, 0, 0, 1);
            }
        }

        public class EulerIMU
        {
            public double X_Pitch { get; set; }
            public double Y_Roll { get; set; }
            public double Z_Yaw { get; set; }
        }

        public class IMU
        {
            public double QX { get; set; }
            public double QY { get; set; }
            public double QZ { get; set; }
            public double QW { get; set; }
            public EulerIMU ToEuler()
            {
                return ToEuler(this);
            }   

            private static EulerIMU ToEuler(IMU imu)
            {
                if (imu == null)
                    return new EulerIMU()
                    {
                        X_Pitch = 0,
                        Y_Roll = 0,
                        Z_Yaw = 0
                    };

                //C# portation
                double ySqr = imu.QY * imu.QY;
                var euler = new EulerIMU();
                // roll (x-axis rotation)
                double t0 = +2.0 * (imu.QW * imu.QX + imu.QY * imu.QZ);
                double t1 = +1.0 - 2.0 * (imu.QX * imu.QX + ySqr);
                euler.Y_Roll = Math.Atan2(t0, t1);
                euler.Y_Roll *= 180 / Math.PI;

                // pitch (y-axis rotation)
                double t2 = +2.0 * (imu.QW * imu.QY - imu.QZ * imu.QX);
                t2 = ((t2 > 1.0) ? 1.0 : t2);
                t2 = ((t2 < -1.0) ? -1.0 : t2);
                euler.X_Pitch = Math.Asin(t2);
                euler.X_Pitch *= 180 / Math.PI;

                // yaw (z-axis rotation)
                double t3 = +2.0 * (imu.QW * imu.QZ + imu.QX * imu.QY);
                double t4 = +1.0 - 2.0 * (ySqr + imu.QZ * imu.QZ);
                euler.Z_Yaw = Math.Atan2(t3, t4);
                euler.Z_Yaw *= 180 / Math.PI;

                return euler;
            }
        }

        IMU rotation = new IMU();
        private Transform3D GetMatrixTransform(double qX, double qY, double qZ, double qW)
        {
            var matrix = ViewMatrix;
            matrix.Rotate(new Quaternion(qX, qY, qZ, qW));
            if (rotation != null)
                matrix.Rotate(new Quaternion(-rotation.QX, -rotation.QY, -rotation.QZ, rotation.QW));
            return new MatrixTransform3D(matrix);
        }

        private Transform3D GetTransform(Orientation value)
        {
            if (value is null) return null;

            float X = value.Quat0.X / (float)10000.0;
            float Y = value.Quat0.Y / (float)10000.0;
            float Z = value.Quat0.Z / (float)10000.0;
            float tmp = (float)(1 - (Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2)));
            float W = (float)Math.Sqrt(tmp);

            return GetMatrixTransform(X, Y, Z, W);
            // return GetTransform(GetAngle(value.Quat0.X), GetAngle(value.Quat0.Y), GetAngle(value.Quat0.Z));
        }

        private double GetAngle(short value)
        {
            return (value > 0) ? 0.01 * value : -0.01 * value;
        }
        private Transform3D GetTransform(double angleX, double angleY, double angleZ)
        {
            var transform = new Transform3DGroup();
            transform.Children.Add(
                new RotateTransform3D()
                {
                    Rotation = new AxisAngleRotation3D()
                    {
                        Axis = new System.Windows.Media.Media3D.Vector3D(1, 0, 0),
                        Angle = angleX
                    }
                });
            transform.Children.Add(
                new RotateTransform3D()
                {
                    Rotation = new AxisAngleRotation3D()
                    {
                        Axis = new System.Windows.Media.Media3D.Vector3D(0, 1, 0),
                        Angle = angleY
                    }
                });
            transform.Children.Add(
                new RotateTransform3D()
                {
                    Rotation = new AxisAngleRotation3D()
                    {
                        Axis = new System.Windows.Media.Media3D.Vector3D(0, 0, 1),
                        Angle = angleZ
                    }
                });
            return transform;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => DependencyProperty.UnsetValue;
    }
    public class OrientationViewModel : INotifyPropertyChanged
    {
        ISource<Orientation> Source { get; set; }
        public Orientation orientation;

        public event PropertyChangedEventHandler PropertyChanged;

        public Orientation Orientation
        {
            get { return orientation; }
            set { this.MutateVerbose(ref orientation, value, RaisePropertyChanged()); }
        }


        public OrientationViewModel(ISource<Orientation> source)
        {
            Source = source;
            Source.OnChange += (Orientation value) => Orientation = value;
        }

        internal void Activate()
        {
            Source.Enable();
        }

        internal void Deactivate()
        {
            Source.Disable();
        }

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
