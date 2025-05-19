import React from "react";
import Header from '../../components/Header/header.jsx'
import classes from "./mainPage.module.css"
import image from "../../assets/icons/derevo.svg"
import Footer from '../../components/Footer/footer.jsx'
import { Link } from "react-router-dom";

const MainPage = (props) => {
    return (
      <div className={classes.main_page}>
        <Header/>
        <div className={classes.all_block}>
        
        <img className={classes.image}src={image} alt=""/>
        {/* <svg className={classes.image} xmlns="http://www.w3.org/2000/svg" viewbox = "0 0 630 725" alt="">
          <use href="../../assets/icons/derevo.svg"></use>
        </svg> */}

                  
          <div className={classes.right_block}>
            <text className={classes.kalm_text}>KALM</text>
            <text className={classes.kalm_text2}>Knowledge Assessment</text>
            <text className={classes.kalm_text2}>& Learning Monitoring</text>
            <section className={classes.kalm_text_information}>
              <p>Сервис для повышения качества образования через прозрачную обратную связь между студентами и преподавателями.</p>
              <p>Студенты могут ознакомиться с реальным опытом других, а преподаватели — получить объективную обратную связь и развиваться в профессии.</p>
              <p>Образование начинается с диалога</p>
            </section>
            <div className={classes.claster_buttons}>
                <Link to= "/Registration"><button className={classes.button2}>Регистрация</button></Link>
                <Link to= "/Login"><button className={classes.button1}>Вход</button></Link>
            </div>
          </div>     
        </div>
        <Footer/>
      </div>
    );
  }
  
  export default MainPage;