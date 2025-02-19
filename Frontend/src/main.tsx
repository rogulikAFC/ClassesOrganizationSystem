import './index.css'
import './pages/layouts/MainLayout'
import ReactDOM from 'react-dom/client';
import { BrowserRouter, Route, Routes } from 'react-router';
import { StrictMode } from 'react';
import MainLayout from './pages/layouts/MainLayout';
import App from './App';

import 'bootstrap/dist/css/bootstrap.min.css';
import LoginPage from './pages/LoginPage';
import AccountPage from './pages/ProfilePage';
import StudentPage from './pages/StudentPage';
import TeacherPage from './pages/TeacherPage';

const root = document.getElementById("root");

ReactDOM.createRoot(root!).render(
  <StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path='/' element={<MainLayout />}>
          <Route path='test' element={<App />} />
          <Route path='login' element={<LoginPage />} />
          <Route path='profile' element={<AccountPage />} />
          <Route path='profile/student' element={<StudentPage />} />
          <Route path='profile/teacher' element={<TeacherPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  </StrictMode>
);