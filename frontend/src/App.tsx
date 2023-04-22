import React from 'react';
import './App.css';
import {Routes, Route, BrowserRouter} from "react-router-dom";
import {Layout} from "./Layout";
import {Average} from "./pages/Average";
import {Codes} from "./pages/Codes";
import {Differences} from "./pages/Differences";
import {Extremes} from "./pages/Extremes";
import {NotFound} from "./pages/NotFound";

function App() {
  return (
      <BrowserRouter>
          <Routes>
              <Route path="/" element={<Layout/>}>
                  <Route index element={<Average/>} />
                  <Route path="extremes" element={<Extremes/>}/>
                  <Route path="differences" element={<Differences/>}/>
                  <Route path="codes" element={<Codes/>}/>
                  <Route path="*" element={<NotFound/>}/>
            </Route>
          </Routes>
      </BrowserRouter>
  );
}

export default App;
