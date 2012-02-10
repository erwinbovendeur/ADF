using System;
using Adf.Core.Tasks;
using Adf.Core.Test;
using Adf.Core.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Adf.Test
{
    [TestClass]
    public class BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            TestManager.Clear();
            ValidationManager.Clear();

            OnInitialize();
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        public virtual void OnInitialize() {}
        public virtual void OnCleanup() {}
    }

    public class BaseTest<T> : BaseTest  where T : ITask
    {
        private T task;

        protected T Task
        {
            get { return default(T).Equals(task) ? task : (task = (T) Activator.CreateInstance(typeof (T), ApplicationTask.Main, null)); }
        }
    }
}
