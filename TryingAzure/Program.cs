using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;


namespace TryingAzure
{
    class Program
    {
        static async Task Main()
        {
            await SynthesizeAudioAsync();
        }
        static async Task SynthesizeAudioAsync()
        {
            // Console.WriteLine(Environment.GetEnvironmentVariable("TRYING_AZURE"));
            // var key = Environment.GetEnvironmentVariable("TRYING_AZURE");
            var config = SpeechConfig.FromSubscription("2789ca2bfb3d41d093fe9092c544a8c4", "westus");

            // using var audioConfig = AudioConfig.FromWavFileOutput(Path.Join(Environment.GetEnvironmentVariable("HOME"), "Downloads", "speech.wav"));
            // using var synthesizer = new SpeechSynthesizer(config, audioConfig);
            // await synthesizer.SpeakTextAsync("A simple test to write to a file.");

            using var synthesizer = new SpeechSynthesizer(config);
            await synthesizer.SpeakTextAsync("Synthesizing directly to speaker output.");
            // var text = "Synthesizing directly to speaker output.";
            // using (var synthesizer = new SpeechSynthesizer(config))
            // {
            //     // Receive a text from console input and synthesize it to speaker.
            //     // Console.WriteLine("Type some text that you want to speak...");
            //     // Console.Write("> ");
            //     // string text = Console.ReadLine();

            //     using (var result = await synthesizer.SpeakTextAsync(text))
            //     {
            //         if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            //         {
            //             Console.WriteLine($"Speech synthesized to speaker for text [{text}]");
            //         }
            //         else if (result.Reason == ResultReason.Canceled)
            //         {
            //             var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
            //             Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

            //             if (cancellation.Reason == CancellationReason.Error)
            //             {
            //                 Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
            //                 Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
            //                 Console.WriteLine($"CANCELED: Did you update the subscription info?");
            //             }
            //         }
            //     }

            //     // This is to give some time for the speaker to finish playing back the audio
            //     // Console.WriteLine("Press any key to exit...");
            //     // Console.ReadKey();
            // }

            // config.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Audio16Khz128KBitRateMonoMp3);

            // using var synthesizer = new SpeechSynthesizer(config, null);

            // var ssml = File.ReadAllText("./ssml.xml");
            // var result = await synthesizer.SpeakSsmlAsync(ssml);
            // // var result = await synthesizer.SpeakTextAsync("Customizing audio output format.");

            // using var stream = AudioDataStream.FromResult(result);
            // await stream.SaveToWaveFileAsync(Path.Join(Environment.GetEnvironmentVariable("HOME"), "Downloads", "speech.mp3"));
        }
    }
}


// curl --location --request POST 'https://westus.tts.speech.microsoft.com/cognitiveservices/v1' \
// --header 'Ocp-Apim-Subscription-Key: 2789ca2bfb3d41d093fe9092c544a8c4' \
// --header 'Content-Type: application/ssml+xml' \
// --header 'X-Microsoft-OutputFormat: audio-16khz-128kbitrate-mono-mp3' \
// --header 'User-Agent: curl' \
// --data-raw '<speak version='\''1.0'\'' xml:lang='\''en-US'\''>
//     <voice xml:lang='\''en-US'\'' xml:gender='\''Female'\'' name='\''en-US-AriaRUS'\''>
//         my voice is my passport verify me
//     </voice>
// </speak>' > output.wav

// curl --location --request POST 'https://westus.tts.speech.microsoft.com/cognitiveservices/v1' \
// --header 'Ocp-Apim-Subscription-Key: 2789ca2bfb3d41d093fe9092c544a8c4' \
// --header 'Content-Type: application/ssml+xml' \
// --header 'X-Microsoft-OutputFormat: audio-48khz-192kbitrate-mono-mp3' \
// --header 'User-Agent: curl' \
// -d @ssml.xml > output.was