import React, { useState } from "react";
import classes from "../DropList/DropList.module.css"

const DropList = ({handleTimeChange}) => {

    return (
      <div>
        <select className={classes.list_city} onChange={handleTimeChange}>
          <option value=""></option>
          <option value=""></option>
          <option value=""></option>
        </select>
      </div>
    );
  };
  
  export default DropList;