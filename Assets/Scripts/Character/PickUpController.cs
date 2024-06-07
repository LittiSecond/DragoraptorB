using Dragoraptor.Interfaces.Character;
using Dragoraptor.Interfaces.Score;
using Dragoraptor.Models;


namespace Dragoraptor.Character
{
    public class PickUpController : IItemCollector
    {

        private IEnergyStore _energyController;
        private IScoreCollector _scoreCollector;
        private ISatietyCollector _satietyCollector;


        public PickUpController(IEnergyStore energyStore, 
            IScoreCollector scoreCollector, 
            ISatietyCollector satietyCollector)
        {
            _energyController = energyStore;
            _scoreCollector = scoreCollector;
            _satietyCollector = satietyCollector;
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
                case ResourceType.Satiety:
                    _satietyCollector.AddSatiety(resource.Amount);
                    isPicked = true;
                    break;
            }

            return isPicked;
        }
        
        #endregion
        
        
    }
}