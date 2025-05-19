import PostSurveys from '../../components/PostSurveys/PostSurveys.jsx'
import Header from '../../components/Header/header.jsx'
import classes from "./surveyPage.module.css"
import Footer from '../../components/Footer/footer.jsx'
import React, { useState } from "react";
import Cookies from 'js-cookie';

const SurveyPage = (props) => {
    const [survey, setSurvey] = useState([]);

    return (
        <div className={classes.page}>
            <Header/>
            <div className={classes.all_block}>
                <text className={classes.text1}>Мои Опросы</text>
                <div className={classes.middle_form}>
                    {survey.length > 0 ? (
                        <PostSurveys survey={survey} key={survey.Id} />
                    ) : (
                        <p className={classes.not_found}>У вас нет опросов!</p>
                    )}
                </div>
            </div>
            <Footer/>       
        </div>
    )
}

export default SurveyPage