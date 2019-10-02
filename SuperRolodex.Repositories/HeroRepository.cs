using System;
using System.Collections.Generic;
using Couchbase.Lite;
using Couchbase.Lite.Query;
using SuperRolodex.Core.Models;
using SuperRolodex.Core.Repositories;

namespace SuperRolodex.Repositories
{
    public class HeroRepository : IHeroRepository
    {
        const string databaseName = "heroes";

        IQuery _heroesQuery;
        ListenerToken _heroesQueryToken;

        protected DatabaseManager _databaseManager;
        protected DatabaseManager DatabaseManager
        {
            get
            {
                if (_databaseManager == null)
                {
                    _databaseManager = new DatabaseManager(databaseName);
                }

                return _databaseManager;
            }
        }

        public void SaveHero(Hero hero)
        {
            DatabaseManager.Database.Save(hero.ToMutableDocument($"hero::{hero.Id}"));
        }

        public List<Hero> GetHeroes(Action<List<Hero>> heroesUpdated)
        {
            var heroes = new List<Hero>();

            try
            {
                _heroesQuery = QueryBuilder
                                .Select(SelectResult.All())
                                .From(DataSource.Database(DatabaseManager.Database))
                                .Where((Expression.Property("type").EqualTo(Expression.String("hero"))));

                if (heroesUpdated != null)
                {
                    _heroesQueryToken = _heroesQuery.AddChangeListener((object sender, QueryChangedEventArgs e) =>
                    {
                        if (e?.Results != null && e.Error == null)
                        {
                            heroes = e.Results.AllResults()?.ToObjects<Hero>() as List<Hero>;

                            if (heroes != null)
                            {
                                heroesUpdated.Invoke(heroes);
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HeroRepository GetHeroesAsync Exception: {ex.Message}");
            }

            return heroes;
        }

        public void DeleteHero(Hero hero)
        {
            var document = DatabaseManager.Database.GetDocument($"hero::{hero.Id}");

            if (document != null)
            {
                DatabaseManager.Database.Delete(document);
            }
        }

        #region Replication
        public void StartReplication() => DatabaseManager.StartReplication();

        public void StopReplication() => DatabaseManager.StopReplication();
        #endregion

        public void Dispose()
        {
            _heroesQuery.RemoveChangeListener(_heroesQueryToken);
            _heroesQuery = null;

            DatabaseManager?.Dispose();
        }
    }
}
