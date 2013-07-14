using System;
using System.Diagnostics.Contracts;

namespace Maraytr.RayCasting {
	public struct IntegrationState {

		public ushort CurrentStep;

		public ushort StepsCountSqrt;


		public IntegrationState(ushort currentStep, ushort stepsCountSqrt) {
			CurrentStep = currentStep;
			StepsCountSqrt = stepsCountSqrt;
		}

		public IntegrationState(int currentStep, int stepsCountSqrt) {
			Contract.Requires<ArgumentOutOfRangeException>(currentStep >= 0 && currentStep <= ushort.MaxValue);
			Contract.Requires<ArgumentOutOfRangeException>(stepsCountSqrt >= 0 && stepsCountSqrt <= ushort.MaxValue);
			CurrentStep = (ushort)currentStep;
			StepsCountSqrt = (ushort)stepsCountSqrt;
		}

	}
}
