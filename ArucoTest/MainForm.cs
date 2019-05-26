using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Aruco;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace ArucoTest
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();


            _detectorParameters = DetectorParameters.GetDefault();

            try
            {
                _capture = new VideoCapture();
                if (!_capture.IsOpened)
                {
                    _capture = null;
                    throw new NullReferenceException("Unable to open video capture");
                }
                else
                {
                    _capture.ImageGrabbed += ProcessFrame;
                }
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }

        private VideoCapture _capture = null;
        private bool _captureInProgress;
        private bool _useThisFrame = false;

        int markersX = 4;
        int markersY = 4;
        int markersLength = 80;
        int markersSeparation = 30;

        private Dictionary _dict;

        private Dictionary ArucoDictionary
        {
            get
            {
                if (_dict == null)
                    _dict = new Dictionary(Dictionary.PredefinedDictionaryName.Dict4X4_100);
                return _dict;
            }

        }

        private GridBoard _gridBoard;
        private GridBoard ArucoBoard
        {
            get
            {
                if (_gridBoard == null)
                {
                    _gridBoard = new GridBoard(markersX, markersY, markersLength, markersSeparation, ArucoDictionary);
                }
                return _gridBoard;
            }
        }

        

        Mat _frame = new Mat();
        Mat _frameCopy = new Mat();

        Mat _cameraMatrix = new Mat();
        Mat _distCoeffs = new Mat();
        Mat rvecs = new Mat();
        Mat tvecs = new Mat();

        private VectorOfInt _allIds = new VectorOfInt();
        private VectorOfVectorOfPointF _allCorners = new VectorOfVectorOfPointF();
        private VectorOfInt _markerCounterPerFrame = new VectorOfInt();
        private Size _imageSize = Size.Empty;

        private DetectorParameters _detectorParameters;

        private void ProcessFrame(object sender, EventArgs arg)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                _capture.Retrieve(_frame, 0);
                _frame.CopyTo(_frameCopy);

                //cameraImageBox.Image = _frame;
                using (VectorOfInt ids = new VectorOfInt())
                using (VectorOfVectorOfPointF corners = new VectorOfVectorOfPointF())
                using (VectorOfVectorOfPointF rejected = new VectorOfVectorOfPointF())
                {
                    //DetectorParameters p = DetectorParameters.GetDefault();
                    ArucoInvoke.DetectMarkers(_frameCopy, ArucoDictionary, corners, ids, _detectorParameters, rejected);
                    
                    if (ids.Size > 0)
                    {
                       // ArucoInvoke.RefineDetectedMarkers(_frameCopy, ArucoBoard, corners, ids, rejected, null, null, 10, 3, true, null, _detectorParameters);
                       
                       ArucoInvoke.DrawDetectedMarkers(_frameCopy, corners, ids, new MCvScalar(0, 255, 0));

                        if (!_cameraMatrix.IsEmpty && !_distCoeffs.IsEmpty)
                        {
                            //ArucoInvoke.EstimatePoseSingleMarkers(corners, 0.5f, _cameraMatrix, _distCoeffs, rvecs, tvecs);
                            ArucoInvoke.EstimatePoseSingleMarkers(corners, markersLength, _cameraMatrix, _distCoeffs, rvecs, tvecs);
                            for (int i = 0; i < ids.Size; i++)
                            {
                                using (Mat rvecMat = rvecs.Row(i))
                                using (Mat tvecMat = tvecs.Row(i))
                                using (VectorOfDouble rvec = new VectorOfDouble())
                                using (VectorOfDouble tvec = new VectorOfDouble())
                                {
                                    double[] values = new double[3];
                                    rvecMat.CopyTo(values);
                                    rvec.Push(values);
                                    tvecMat.CopyTo(values);
                                    tvec.Push(values);

                                    if (ids.Size == 1)
                                    {
                                        coordinates(values, ids[i]);
                                    }


                                    ArucoInvoke.DrawAxis(_frameCopy, _cameraMatrix, _distCoeffs, rvec, tvec,
                                       markersLength * 0.3f);

                                }
                            }
                        }
                        else if (ids.Size >= 12)
                        {
                            _allCorners.Push(corners);
                            _allIds.Push(ids);
                            _markerCounterPerFrame.Push(new int[] { corners.Size });
                            _imageSize = _frame.Size;
                            double repError = ArucoInvoke.CalibrateCameraAruco(_allCorners, _allIds, _markerCounterPerFrame, ArucoBoard, _imageSize,
                               _cameraMatrix, _distCoeffs, null, null, CalibType.Default, new MCvTermCriteria(30, double.Epsilon));
                            _allCorners.Clear();
                            _allIds.Clear();
                            _markerCounterPerFrame.Clear();
                            _imageSize = Size.Empty;
                        }
                    }
                    cameraImageBox.Image = _frameCopy.Clone();
                }
            }
        }

        private void coordinates(double[] values, int ids)
        {
            this.lb_result.BeginInvoke((MethodInvoker)(() => this.lb_result.Text = "id marker: [" + ids + "] x = " + values[0] + " y = " + values[1] + " z = " + values[2]));
        }

        private void cameraButton_Click(object sender, EventArgs e)
        {
            if (_capture != null)
            {
                if (_captureInProgress)
                {
                    //stop the capture
                    cameraButton.Text = "Start Capture";
                    _capture.Pause();

                }
                else
                {
                    //start the capture
                    cameraButton.Text = "Stop Capture";
                    _capture.Start();
                }

                _captureInProgress = !_captureInProgress;
            }
        }

        private void useThisFrameButton_Click(object sender, EventArgs e)
        {
            _useThisFrame = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
