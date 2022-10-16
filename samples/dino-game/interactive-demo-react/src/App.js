import React, { useEffect } from 'react';
import logo from './logo.svg';
import { Counter } from './features/counter/Counter';
import { DinoGame } from './features/dinoGame/DinoGame';
import { Guides } from './features/guides/Guides';
import './App.css';

function App() {
  useEffect(() => {
    document.body.classList.add('offline');
  })
  return (
    <div className="App">
      <Guides />
      <DinoGame />
    </div>
  );
}

export default App;
