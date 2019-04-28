using System.Collections.Generic;
using System;

namespace Chaotx.Minestory {
    [Serializable]
    public class Highscore : IComparable<Highscore> {
        public string Name {get; set;}
        public int MinesHit {get;}
        public int TotalMines {get;}
        public TimeSpan Time {get;}
        public DateTime TimeStamp {get;}
        public GameSettings Settings {get;}

        public Highscore(
            string name, int minesHit, int totalMines,
            TimeSpan time, GameSettings settings)
        {
            Name = name;
            MinesHit = minesHit;
            TotalMines = totalMines;
            Time = time;
            Settings = settings;
            TimeStamp = DateTime.Now;
        }

        public int CompareTo(Highscore score) {
            int diff1 = (((int)Settings.Difficulty)+1)%4;
            int diff2 = (((int)score.Settings.Difficulty)+1)%4;

            if(diff1 == diff2) {
                float mines1 = MinesHit/(float)TotalMines;
                float mines2 = score.MinesHit/(float)score.TotalMines;

                if(mines1 == mines2) {
                    if(Time.Equals(score.Time))
                        return TimeStamp.CompareTo(score.TimeStamp);
                        
                    return Time.CompareTo(score.Time);
                }

                return mines1.CompareTo(mines2);
            }

            return diff2.CompareTo(diff1);
        }

        public static bool operator >(Highscore score1, Highscore score2) {
            return score1.CompareTo(score2) == 1;
        }

        public static bool operator <(Highscore score1, Highscore score2) {
            return score1.CompareTo(score2) == -1;
        }

        public static bool operator >=(Highscore score1, Highscore score2) {
            return score1.CompareTo(score2) >= 0;
        }

        public static bool operator <=(Highscore score1, Highscore score2) {
            return score1.CompareTo(score2) <= 0;
        }

        public override bool Equals(object obj) {
            if(obj == this) return true;
            if(obj == null || obj.GetType() != GetType())
                return false;

            Highscore score = obj as Highscore;
            return score.Name.Equals(Name)
                && score.MinesHit.Equals(MinesHit)
                && score.TotalMines.Equals(TotalMines)
                && score.Time.Equals(Time)
                && score.Settings.Equals(Settings)
                && score.TimeStamp.Equals(TimeStamp);
        }

        public override int GetHashCode() {
            int hash = 13;
            int prim = 11;

            hash = (hash*prim) ^ Name.GetHashCode();
            hash = (hash*prim) ^ MinesHit.GetHashCode();
            hash = (hash*prim) ^ Time.GetHashCode();
            hash = (hash*prim) ^ Settings.GetHashCode();
            hash = (hash*prim) ^ TimeStamp.GetHashCode();
            return hash;
        }
    }
}