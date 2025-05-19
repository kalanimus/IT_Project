import Header from '../../components/Header/header.jsx'
import classes from "./loginPage.module.css"
import Footer from '../../components/Footer/footer.jsx'
import {useNavigate} from "react-router-dom";
import React, { useCallback } from 'react';
import Cookies from 'js-cookie';


const LoginPage = (props) => {

    const navigate = useNavigate()

    const fetchLogin = useCallback(async (login, password) => {
        const response = await fetch(
            "http://localhost:5113/api/auth/login", {
                method: "post",
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    username: login,
                    password: password,
                })
            }
        )
        const data = await response.json()
        if (response.ok){
            // Cookies.set("auth", data.Token)
            // navigate("/Cabinet")


        } else {
            console.log("Неправильный пароль или login")
        }
        return data;

    }, [])

    const handleSubmit = (event) => {
        event.preventDefault();
        const login = event.target.login.value
        const password = event.target.password.value
        fetchLogin(login, password)
        .then((response) => {
            console.log(response);
        }) 
    };

    return(
        <div className={classes.page}>
            <Header/>
            <div className={classes.container_registration}>
                <div className={classes.form}>
                    <h1 className={classes.head_word}>Вход</h1>
                    <form onSubmit={handleSubmit} className={classes.wrapper_inputs}>
                        <label>
                            <p>Логин</p>                               
                            <p><input name="login" type="text" placeholder="abc23s01"
                                      className={classes.input}/></p>
                        </label>
                        <label>
                            <p>Пароль</p>
                            <p><input name="password" type="password" placeholder="qwerty12345"
                                      className={classes.input}/></p>
                        </label>

                        <button type="submit" className={classes.button_sent}>Войти</button>
                    </form>
                </div>
            </div>
            <Footer/>
         </div>
    )
}

export default LoginPage;
