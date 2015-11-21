# PulseSequencer

A pulse-based sequencer for Unity3d.

## See it in action!

[Derelict Computer](http://derelict.computer)

## Info

The PulseSequencer allows composers to create 3d, polyrhythmic music using Unity's built-in audio system. It also allows for triggering visual effects, such as animations or light lerps in sync with the music.

You can clone or fork this repo, or [download the Unity package](http://charliehuge.com/publicdownloads/PulseSequencer.unitypackage) and import it into your project.

To get started, check out the example scene, or:
- Add a Pulse, Pattern, and Sampler to a scene
- Reference the Pulse in the Pattern object
- Reference the Pattern in the Sampler object
- Add a sample and reference an AudioClip in the Sampler object
- Add some steps to the Pattern object

## Platform Support
- Windows: verified working
- Mac: verified working
- Linux: theoretically works, but not verified
- iOS: theoretically works, but not verified
- Android: theoretically works, but not verified
- HTML5/WebGL Preview: basic sampling works, but volume envelopes do not (pending support for AudioFilters, ETA from Unity unknown)

## Disclaimer

This project is very young, and therefore many safeguards and editor helpers are not implemented. I'm using this in my own projects, and will be updating this repository as I add features in those projects. So please bear with me! If you have any feature requests or find any bugs, please feel free to report them via the issue tracker.

## Wanna help?

If you like using the Pulse Sequencer, and want to help me develop it, that's awesome! There are a few ways you can help:
- Submit bugs and feature requests via the issue tracker. This will help me know what people want to see, and prioritize those things.
- If you're a coder, fork the repo and submit pull requests for features and bug fixes. This gets a lot better if I have help.
- Let me know how you're using it. Seeing your work inspires me to keep going further, and often gives me ideas for new features.
- If you're a dev studio that wants a novel interactive music solution, [hire me](http://derelict.computer/consulting.html)!
