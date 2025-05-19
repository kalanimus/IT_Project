import React from "react";
import {BrowserRouter, Routes, Route} from "react-router-dom"
import Main from "./pages/MainPage/mainPage";
import Search from "./pages/SearchPage/searchPage";
import './assets/Fonts/fonts.css';
import MainPage from "./pages/MainPage/mainPage";
import RegistrationPage from './pages/Registration/registrationPage.jsx';
import LoginPage from './pages/Login/loginPage.jsx';
import SurveyPage from "./pages/SurveyPage/surveyPage";

function App(){
    return(
        <BrowserRouter>
            <Routes>
                <Route element={<Main />} path="/" />
                <Route element={<MainPage/>} path="/Main"></Route>
                {/* <Route element={<Teachers/>} path="/Teachers"></Route> */}
                <Route element={<RegistrationPage/>} path="/Registration"></Route>
                <Route element={<LoginPage/>} path="/Login"></Route>
                <Route element={<SurveyPage/>} path="/Survey"></Route>
            </Routes>
        </BrowserRouter>
    )
}
export default App;