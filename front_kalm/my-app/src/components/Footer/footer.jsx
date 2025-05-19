import React from "react";
import classes from './footer.module.css';
import { Link } from "react-router-dom";
import baumanLogo from "../../assets/icons/baumanLogo.svg"

const Footer = (props) =>{
    return (
        <footer className= {classes.footer}>      
            <div className = {classes.container_footer}>
                <div className={classes.left_container}>
                    <img className={classes.baumanLogo}src={baumanLogo} alt=""/>
                </div>
                <div className={classes.right_container}>
                    
                    <div>
                        <h4>О проекте</h4>
                        <ul className= {classes.list}>
                            <li>KALM - Сервис для повышения качества образования через прозрачную обратную связь между студентами и преподавателями. Студенты могут ознакомиться с реальным опытом других, а преподаватели - получить объективную обратную связь и развиваться в профессии. Образование начинается с диалога.</li>
                        </ul>
                    </div>
                    <div className={classes.info}>
                        <div>
                            <h4>Информация</h4>
                            <ul className= {classes.list_1}>
                                <li><a href="">О проекте</a></li>
                                <li><Link to= "/Hacktons">Преподаватели</Link></li> 
                                <li><a href="">Опросы</a></li>
                            </ul>
                        </div>

                        <div>
                            <h4>Социальные сети</h4>
                            <ul className= {classes.list_2}>
                                <li><a href="">VKONTAKTE</a></li>
                                <li><a href="">Telegram</a></li>
                            </ul>
                        </div>

                        <div>
                            <h4>Адрес</h4>
                            <ul className= {classes.list_3}>
                                <li>105005, Москва, 2-я Бауманская ул., 
                                    д. 5, стр. 1</li>
                            </ul>
                        </div>

                        <div>
                            <h4>Контакты</h4>
                            <ul className= {classes.list_4}>
                                <li>8 (800) 555-35-35</li>
                                <li>shact.project@bmstu.ru</li>
                            </ul>
                        </div>
                        

                    </div>

                </div>
            </div>
            <div className={classes.copy}>2024. Все права защищены</div>
        </footer>

    );
};

export default Footer