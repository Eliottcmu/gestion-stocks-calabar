import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { useState } from 'react';
import Home from './pages/Home';
import Stock from './pages/Stock';
import Statistiques from './pages/Statistiques';
import Ventes from './pages/Ventes';
import Tresorerie from './pages/Tresorerie';
import NavBar from './components/NavBar/NavBar';


const App = () => {
  const [page, setPage] = useState('Home');
  return (
    <BrowserRouter>
      <NavBar setPage={setPage} />
      <Routes>
        <Route path="/" element={<Home setPage={setPage} />} />
        <Route path="/stock" element={<Stock setPage={setPage} />} />
        <Route path="/statistiques" element={<Statistiques setPage={setPage} />} />
        <Route path="/ventes" element={<Ventes setPage={setPage} />} />
        <Route path="/tresorerie" element={<Tresorerie setPage={setPage} />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
