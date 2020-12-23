using System;
using System.Linq;
using System.Threading.Tasks;

namespace Square
{
    public class ScoreData
    {
        private static ScoreData[] score;
        private static ScoreData[] onlineScore;
        private static DateTime lastSynchro = DateTime.Now.AddMinutes(-5);

        public string LevelName { get; set; }

        public string PersonId { get; set; }

        public int Score { get; set; }

        public static async Task<ScoreData[]> AllAsync()
        {
            score ??= Store.LoadLevelScore();

            if (lastSynchro < DateTime.Now.AddMinutes(-5))
            {
                lastSynchro = DateTime.Now;
                onlineScore = await FirebaseHelper.GetAll().ConfigureAwait(false);
                UpdateScores();
            }

            return score;
        }

        public static async void UpdateScores()
        {
            if (onlineScore != null)
            {
                for (int i = 0; i < score.Length; i++)
                {
                    if (score[i].Score < onlineScore[i].Score)
                        score[i] = onlineScore[i];
                    else if (score[i].Score != onlineScore[i].Score)
                        await FirebaseHelper.UpdateScore(score[i].Score, $"level{i/7 + 1}{i%7}").ConfigureAwait(false);
                }
            }
        }

        public static async void Compare(byte epizoda, byte cisloUrovne, short skore)
        {
            if (!FirebaseHelper.Checked)
            {
                string name = $"level{epizoda}{cisloUrovne}";
                ScoreData local = AllAsync().Result.First(s => s.LevelName == name);
                if (local.Score < skore)
                {
                    score.First(s => s.LevelName == name).Score = skore;
                    Store.SaveLevelScore(score);
                    await FirebaseHelper.UpdateScore(skore, name).ConfigureAwait(false);
                }                

                FirebaseHelper.Checked = true;
            }
        }

        public static bool GetRecord(Level uroven, out int rekord)
        {
            ScoreData data = score.First(s => s.LevelName == $"level{uroven.Epizoda}{uroven.CisloUrovne}"); 
            rekord = data.Score;

            return FirebaseHelper.PersonId.ToString() == data.PersonId;
        }
    }
}