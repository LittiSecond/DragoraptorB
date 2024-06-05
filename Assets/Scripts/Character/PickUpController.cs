using Dragoraptor.Interfaces.Character;
using Dragoraptor.Interfaces.Score;
using Dragoraptor.Models;


namespace Dragoraptor.Character
{
    public class PickUpController : IItemCollector
    {

        private IEnergyStore _energyController;
        private IScoreCollector _scoreCollector;


        public PickUpController(IEnergyStore energyStore, IScoreCollector scoreCollector)
        {
            _energyController = energyStore;
            _scoreCollector = scoreCollector;
        }

        #region IItemCollector
        
        public bool PickUp(PickableResource resource)
        {
            bool isPicked = false;

            switch (resource.Type)
            {
                case ResourceType.Energy:
                    _energyController.AddEnergy(resource.Amount);
                    isPicked = true;
                    break;
                case ResourceType.Score:
                    _scoreCollector.AddScore(resource.Amount);
                    isPicked = true;
                    break;
            }

            return isPicked;
        }
        
        #endregion
        
        
    }
}