using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace SlipThrough2.Managers
{
    public class AudioManager
    {
        // use .wav files
        private static List<SoundEffect> soundEffects;
        private static SoundEffectInstance soundEffectInstance;
        private static bool soundWasPlaying;
        private static int bufferIterations;
        public static bool enableSoundEffects = true;

        // Method to load sound effect data
        public static void LoadSoundEffects(List<SoundEffect> SoundEffects)
        {
            soundEffects = SoundEffects;
            soundEffectInstance = SoundEffects[1].CreateInstance(); // This is just for walking
        }

        // Method to play the loaded sound effect
        public static void PlaySoundOnce(string name)
        {
            if (!enableSoundEffects)
                return;

            System.Console.WriteLine("Playing Sound");

            int index = name switch
            {
                "door" => 0,
                "walking" => 1,
                "death" => 2,
                "attack" => 3,
                _ => throw new System.NotImplementedException()
            };

            // Ensure sound effect is not null before playing
            soundEffects[index].Play();
        }

        public static void PlayLoopedWalkingSound(bool doPlay)
        {
            if (!enableSoundEffects)
                return;

            soundEffectInstance.IsLooped = true;

            if (doPlay)
            {
                // Play the sound effect instance
                if (soundWasPlaying)
                    soundEffectInstance.Resume();
                else
                    soundEffectInstance.Play();

                soundWasPlaying = true;
                bufferIterations = 0;
            }
            else
            {
                // Stop playing but only after a slight delay
                if (bufferIterations > 2)
                    StopLoopingSound();
                else
                    bufferIterations++;
            }
        }

        public static void StopLoopingSound()
        {
            soundEffectInstance.Pause();
            soundWasPlaying = false;
            bufferIterations = 0;
        }
    }
}
