using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoodRock.Models
{
    public class UnitViewModel
    {
        public SavedGame SavedGame { get; set; }
        public List<Models.Human> HumanUnits { get { return Utilities.UnitManager.HumanUnits.OrderBy(x => x.UnitName).ToList(); } }
        public List<Models.Animal> AnimalUnits { get { return Utilities.UnitManager.AnimalUnits.OrderBy(x => x.UnitName).ToList(); } }
        public List<Models.Enemy> EnemyUnits { get { return Utilities.UnitManager.EnemyUnits.OrderBy(x => x.UnitName).ToList(); } }

        public Models.Human SelectedHuman { get; set; }
    }
}
