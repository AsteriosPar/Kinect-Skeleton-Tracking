## KINECT SKELETON TRACKING

### MOTIVATION

Human pose estimation has been a long standing problem in computer vision with applications
in Human Computer Interaction, motion capture and activity recognition. Complexities arise
due to the high dimension of the search space, large number of degree of freedoms involved
and the constraints involved such as no body part penetration and disallowing impossible positions. Other challenges include variation of cluttered background, body parameters, illumination changes

The skeleton tracker is a great way to capture a user's movement and actions. The aim of this project is to estimate the body pose and orientation, and combine the data to provide accurate information on the user's posture. This project utilizes the Kinect v2 sensor and can extract comprehensive joints information from all parts of the body. This project also has a possibility to be further extended to be used for continuous full body gait tracking.

### METHODOLOGY

1. Kinect Initialization, we initialize and keep an array of all joints in the last seen body. Each joint has one Vector in the array desribing it's 3D location        in camera coordinates.
2. To get the joint tracking information we grab the NUI_SKELETON_FRAME stream. However, the tracking can get noisy so we incorporate a smoothing function.
3. Once we have a valid tracked skeleton, then we can just copy all the joint data into our array of joint positions, we also track the joint states of each            individual joint incase the Kinect may not be able to  track all of the joints.
4. We then use the OpenGL library to render the skeleton lines to connect the        joints.
5. For future use this project can be used to classify the postures and also also     be used for continuous full body gait tracking.


![kinect_skeleton_tracking](https://user-images.githubusercontent.com/30382104/59148619-44ff4780-89d9-11e9-8088-08535e0a4fc2.gif)


### PREREQUISITES TO BE INSTALLED
  1. Microsoft Kinect SDK 2.0
  2. Visual Studio 2015
 
### USAGE
  1. Run KinectStreams.sln
  2. Click the Skeleton Record Button to start and record the skeleton stream.
  3. Click the Color Button to get the color stream.
  4. Click Stop Button to stop the recording and save the joints position and orientation data to a csv file.
