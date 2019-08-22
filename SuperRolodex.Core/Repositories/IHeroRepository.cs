using System;
using System.Collections.Generic;
using SuperRolodex.Core.Models;

namespace SuperRolodex.Core.Repositories
{
    public interface IHeroRepository
    {
        void SaveHero(Hero hero);
        List<Hero> GetHeroes(Action<List<Hero>> heroesUpdated);
        void DeleteHero(Hero hero);

        void StartReplication();
        void StopReplication();
    }
}
