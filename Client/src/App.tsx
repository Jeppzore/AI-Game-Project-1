import { BrowserRouter, Routes, Route } from 'react-router-dom';
import EnemyListPage from './pages/EnemyListPage';
import EnemyDetailPage from './pages/EnemyDetailPage';
import './App.css';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<EnemyListPage />} />
        <Route path="/enemy/:id" element={<EnemyDetailPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;

