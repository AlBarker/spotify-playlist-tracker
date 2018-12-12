import React, { Component } from 'react';
import logo from './logo.svg';
import './App.css';
import { NowPlaying}  from './Components/NowPlaying/NowPlaying.js'

class App extends Component {
  render() {
    return (
      <div className="App">
        <NowPlaying />
      </div>
    );
  }
}

export default App;
