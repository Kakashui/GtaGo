using System;

namespace Whistler.Jobs.ImpovableJobs
{
    public class ImprovableJobState
    {
        public ImprovableJobType JobType { get; set; }

        public int CurrentLevel { get; set; }

        public int StagesPassed { get; set; }
        
        public void SetCurrentLevel<T>(T levelToSet) where T : Enum
        {
            CurrentLevel = Convert.ToInt32(levelToSet);
            StagesPassed = 0;
        }

        public T GetCurrentLevel<T>() where T : Enum
        {
            return (T) Enum.ToObject(typeof(T) , CurrentLevel);
        }

        public ImprovableJobState(ImprovableJobType jobType)
        {
            JobType = jobType;
        }
    }
}