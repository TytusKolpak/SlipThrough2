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

        // Method to load sound effect data
        public static void LoadSoundEffects(List<SoundEffect> SoundEffects)
        {
            soundEffects = SoundEffects;
            soundEffectInstance = SoundEffects[1].CreateInstance(); // This is just for walking
        }

        // Method to play the loaded sound effect
        public static void PlaySoundOnce(string name)
        {
            System.Console.WriteLine("Playing Sound");

            int index = name switch
            {
                "door" => 0,
                "walking" => 1,
                "death" => 2,
                _ => throw new System.NotImplementedException()
            };

            // Ensure sound effect is not null before playing
            soundEffects[index].Play();
        }

        public static void PlayLoopedWalkingSound(bool doPlay)
        {
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
                {
                    soundEffectInstance.Pause();
                    soundWasPlaying = false;
                    bufferIterations = 0;
                }
                else
                    bufferIterations++;
            }
        }
    }
}
