// Copyright 2012 - 2013 dbones.co.uk (David Rundle)
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace Boxes.Integration.Tasks
{
    using System;
    using System.Linq;
    using Boxes.Tasks;
    using ContainerSetup;

    internal class RegisterTypesWithIocPackageTask : IBoxesTask<ProcessPackageContext>
    {
        private readonly IBoxesContainerSetup _boxesContainerSetup;
        private PipilineExecutor<Type> _pipilineExecutor;

        private int _numberOfRegistrations =-1;

        public RegisterTypesWithIocPackageTask(IBoxesContainerSetup boxesContainerSetup)
        {
            _boxesContainerSetup = boxesContainerSetup;
        }

        public void UpdateTasksAsRequired()
        {
            var currentNumberOfRegistrations = _boxesContainerSetup.Registrations.Count();
            
            if (currentNumberOfRegistrations == _numberOfRegistrations)
            {
                return;
            }
            _numberOfRegistrations = currentNumberOfRegistrations; 
            _pipilineExecutor = _boxesContainerSetup.Registrations.CreatePipeline();
        }

        public bool CanHandle(ProcessPackageContext item)
        {
            return true;
        }

        public void Execute(ProcessPackageContext item)
        {
            _pipilineExecutor.Execute(item.DependencyTypes).Force();
        }
    }
}