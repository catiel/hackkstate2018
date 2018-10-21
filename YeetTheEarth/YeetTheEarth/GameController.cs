﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeetTheEarth
{
    public class GameController
    {
        private readonly int _monthsToSurvive = 36;
        private Player _player = new Player();
        private Earth _earth = new Earth();
        private EventGenerator _eventGenerator;
        private List<IEvent> _activeEvents;
        private Random _randomizer;
        private int _eventGenerationChance;
        private long _initialPopulation;
        private long _halfPopulation;
        private decimal _initialGDP;
        private decimal _halfGDP;

        public GameController()
        {
            _player = new Player();
            _earth = new Earth();
            _eventGenerator = new EventGenerator(_earth);
            _activeEvents = new List<IEvent>();
            _randomizer = new Random();
            _eventGenerationChance = 30; //Each month, 30% chance of generating a new event
            _initialPopulation = _earth.Population;
            _halfPopulation = (long)((double)_initialPopulation / (double)2);
            _initialGDP = _earth.GDP;
            _halfGDP = (_initialGDP / (decimal)2);
        }

        public void RunGame()
        {
            _player.ShowGameIntroMessage(_earth.Year);

            while ((_earth.Population >= _halfPopulation)
                && (_earth.GDP >= _halfGDP)
                && (_earth.MonthsElapsed < _monthsToSurvive))
            {
                //Show stuff about Earth's current state
                _player.ShowYear(_earth.Year);
                _player.ShowMonth(_earth.CurrentMonth);
                _player.ShowPopulation(_earth.Population);
                _player.ShowPoliticalPoints(_earth.PoliticalPoints);
                _player.ShowTemperature(_earth.Temp);
                _player.ShowSeaLevel(_earth.SeaLevel);
                _player.ShowGDP(_earth.GDP);

                //Generate random events sometimes
                if (_randomizer.Next(100) < _eventGenerationChance)
                {
                    _activeEvents.Add(_eventGenerator.GetEvent());
                }

                //Handle all active events
                if (_activeEvents.Count > 0)
                {
                    //Advance each event one month
                    for (int i = 0; i < _activeEvents.Count; i++)
                    {
                        IEvent thisEvent = _activeEvents[i];
                        //Show event info
                        _player.ShowEventInfo(thisEvent.Name, thisEvent.Description);

                        //Show player event message for this month
                        _player.ShowEventOptions(thisEvent.NextMonth());

                        //Get choice from player
                        _player.ShowEventResult(thisEvent.ChooseOption(_player.GetChoice()));
                    }

                    //Remove events that are done
                    for (int i = _activeEvents.Count - 1; i >= 0; i--)
                    {
                        if (_activeEvents[i].MonthsLeft <= 0)
                        {
                            _activeEvents.RemoveAt(i);
                        }
                    }
                }

                //Show player normal monthly options
                //Get decisions from player

                _earth.NextMonth();
                _player.NextMonth(_earth.CurrentMonth);
            }

            if (_earth.Population < _halfPopulation)
            {
                _player.ShowLosePopulation(_initialPopulation, _earth.Population);
                _player.ShowYear(_earth.Year);
                _player.ShowMonth(_earth.CurrentMonth);
                _player.ShowPopulation(0);
                _player.ShowPoliticalPoints(_earth.PoliticalPoints);
                _player.ShowTemperature(_earth.Temp);
                _player.ShowSeaLevel(_earth.SeaLevel);
                _player.ShowGDP(_earth.GDP);
            }
            else if (_earth.GDP < _halfGDP)
            {
                _player.ShowLoseGDP(_initialGDP, _earth.GDP);
                _player.ShowYear(_earth.Year);
                _player.ShowMonth(_earth.CurrentMonth);
                _player.ShowPopulation(0);
                _player.ShowPoliticalPoints(_earth.PoliticalPoints);
                _player.ShowTemperature(_earth.Temp);
                _player.ShowSeaLevel(_earth.SeaLevel);
                _player.ShowGDP(_earth.GDP);
            }
            else if (_earth.MonthsElapsed >= _monthsToSurvive)
            {
                _player.ShowWin();
                _player.ShowYear(_earth.Year);
                _player.ShowMonth(_earth.CurrentMonth);
                _player.ShowPopulation(_earth.Population);
                _player.ShowPoliticalPoints(_earth.PoliticalPoints);
                _player.ShowTemperature(_earth.Temp);
                _player.ShowSeaLevel(_earth.SeaLevel);
                _player.ShowGDP(_earth.GDP);
            }
            else
            {
                _player.Show("Well you exited the main loop but broke the game. Good job.");
                _player.ShowYear(_earth.Year);
                _player.ShowMonth(_earth.CurrentMonth);
                _player.ShowPopulation(_earth.Population);
                _player.ShowPoliticalPoints(_earth.PoliticalPoints);
                _player.ShowTemperature(_earth.Temp);
                _player.ShowSeaLevel(_earth.SeaLevel);
                _player.ShowGDP(_earth.GDP);
            }

            _player.EndGame();
        }
    }
}
