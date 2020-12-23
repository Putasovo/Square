using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;

namespace Square
{
    public static class FirebaseHelper
    {
        private const int defaultWaitingTime = 3333;
        private static readonly FirebaseClient firebase = new FirebaseClient("https://mighty-square-95319175.firebaseio.com/");

        private static Guid personId;

        public static Guid PersonId 
        {
            get 
            {
                if (personId == Guid.Empty)
                {
                    personId = Store.LoadId();
                }

                return personId;
            } 
        }

        public static bool Checked { get; set; }

        public static async Task<ScoreData[]> GetAll(int waitingTime = defaultWaitingTime)
        {
            return (await firebase.Child("level")
                .OnceAsync<ScoreData>(TimeSpan.FromMilliseconds(waitingTime)).ConfigureAwait(false))
                .Select(item => new ScoreData
                {
                    LevelName = item.Key,
                    Score = item.Object.Score,
                    PersonId = item.Object.PersonId
                }).ToArray();
        }

        public static async Task UpdateScore(int score, string name)
        {
            ScoreData toUpdate = await firebase.Child("level").Child(name)
                .OnceSingleAsync<ScoreData>(TimeSpan.FromMilliseconds(defaultWaitingTime))
                .ConfigureAwait(false);

            if (score > toUpdate.Score)
            {
                await firebase
                  .Child("level")
                  .Child(name)
                  .PutAsync(new ScoreData() { PersonId = PersonId.ToString(), Score = score })
                  .ConfigureAwait(false);
            }            
        }
    }
}
