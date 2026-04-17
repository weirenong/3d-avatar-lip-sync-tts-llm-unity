# 3d-avatar-lip-sync-tts-llm-unity
Real-time 3D avatar system in Unity featuring lip sync, text-to-speech, and LLM-powered dialogue.

This project demonstrates an interactive AI avatar pipeline, combining multiple technologies into a cohesive, real-time experience. It serves as a proof-of-concept for building conversational 3D avatars and can be used as a starting point for developers looking to integrate AI-driven characters into their own Unity applications.

The system leverages LLMUnity to run a locally downloaded language model as the conversational engine. A cross-platform TTS implementation enables native voice generation using the operating system’s built-in text-to-speech capabilities. uLipSync processes the generated audio to drive real-time lip sync animation on the 3D avatar via blendshapes. The Unity-Chan model is used as the character base and is configured according to the uLipSync setup guidelines.

![Demo](demo.gif)

You may select your choice of voice for each platform. Test audio will be played at the start of the play mode.
![Demo](crossplatformtts.png)

Note:
* You will need to initialise LLMUnity on your first run. Refer to the official github for LLMUnity for instructions on how to set up. Open the scene, then select the LLM game object, download the libraries and also the LLM model of your choice.

* MacOS and Windows platforms are supported but it is only tested on MacOS.

* Unity version used : 2022.3.62f2

This repository’s original code is MIT-licensed; third-party assets and dependencies remain under their own licences.
Credits are fully attributed to the following projects:

1. LLMUnity
https://github.com/undreamai/LLMUnity
license: Apache-2.0 license

2. uLipSync
https://github.com/hecomi/uLipSync
license: MIT license

4. Unity-Chan!
https://unity-chan.com
license: Unity-chan License Terms Version 3.0
Unity Technologies Japan/UCL

All license files are included within the distributed versions in this project.
