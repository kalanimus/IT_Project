import React from "react";
import classes from "./PostSurveys.module.css";
import Down from "./Down";

const PostSurveys = ({ survey, onTakeSurvey, onViewAnswers }) => {
    return (
        <div className={classes.survey_list}>
        {survey.map((survey) => (
          <div key={survey.id} className={classes.survey_item}>
            <div>
              <h3 className={classes.survey_title}>{survey.title}</h3>
              <p className={classes.survey_teacher}>Преподаватель: {classes.survey.teacher}</p>
            </div>
            <button
              onClick={() =>
                survey.completed
                  ? onViewAnswers(survey.id)
                  : onTakeSurvey(survey.id)
              }
              className={`survey_button ${survey.completed ? 'completed' : 'not_completed'}`}
            >
              {survey.completed ? 'Посмотреть ответы' : 'Пройти опрос'}
            </button>
          </div>
        ))}
      </div>
    );
}

export default PostSurveys