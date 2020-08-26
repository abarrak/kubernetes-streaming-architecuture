# Motivation

This is a simple experiment to  demonstrate the importance of the study area quantitively, the following experiment is held on cloud environment (AWS) with 3 setups. **The only primary measured dimension is on the output streaming side for CPU utilization cost**, for brevity. A video stream is prerecorded in MPEG-DASH (WebM) format for various adaptive resolutions and bitrates using FFMpeg library. Then, it is served to different clients concurrently with different connectivity and preferences (via JMeter and Selenium WebDriver). The results reveal that an auto-scalable nodes architecture outperformed both dedicated single and 3 nodes, without the need to provision resources in advance.

## Results

The following table summarizes the results:

![](https://raw.githubusercontent.com/abarrak/kubernetes-streaming-architecuture/master/motivation/JMeter/Results.png)

## Details

- Simple DASH Adaptive Streaming for HTML 5 Video tag for many clients at once.

- Tests Streaming output part only.

- A sample video file is processed in Dash WebM prior the experiment and put on Apcahe server to serve to clients.
  1. http://wiki.webmproject.org/adaptive-streaming/instructions-to-playback-adaptive-webm-using-dash
  2. https://developer.mozilla.org/en-US/docs/Web/HTML/DASH_Adaptive_Streaming_for_HTML_5_Video
  3. https://medium.com/@toastui/implementing-adaptive-http-streaming-using-the-web-e2c12d46a38f

- Chrome Selenium Driver is used to simulate many clients and test the scalability.

https://dzone.com/articles/performance-test-with-selenium

The client Page:

```html
<html> 

<video>
  <source src="movie.mpd">
  <source src="movie.webm">
  Your browser does not support the video tag.
</video>
</html>
```

- Which is better: AWS dedicated EC2 instances (i.e. 1 & 3 instances) or ASG instance group ?
  1. https://aws.amazon.com/ec2/
  2. https://docs.aws.amazon.com/autoscaling/ec2/userguide/AutoScalingGroup.html

- FFMPEG WebM transcoding scripts (1 for generating segs & 1 for manifest) + Output.

![1](https://raw.githubusercontent.com/abarrak/kubernetes-streaming-architecuture/master/motivation/JMeter/1.png)
![2](https://raw.githubusercontent.com/abarrak/kubernetes-streaming-architecuture/master/motivation/JMeter/3.png)
![3](https://raw.githubusercontent.com/abarrak/kubernetes-streaming-architecuture/master/motivation/JMeter/4.png)
![4](https://raw.githubusercontent.com/abarrak/kubernetes-streaming-architecuture/master/motivation/JMeter/5.png)
![5](https://raw.githubusercontent.com/abarrak/kubernetes-streaming-architecuture/master/motivation/JMeter/6.png)
