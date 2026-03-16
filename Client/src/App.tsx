import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Header } from './components/Header';
import EnemyListPage from './pages/EnemyListPage';
import EnemyDetailPage from './pages/EnemyDetailPage';
import './App.css';

function App() {
  return (
    <BrowserRouter>
      <Header />
      <main className="main-content">
        <Routes>
          <Route path="/" element={<EnemyListPage />} />
          <Route path="/enemy/:id" element={<EnemyDetailPage />} />
        </Routes>
      </main>
    </BrowserRouter>
  );
}

export default App;

