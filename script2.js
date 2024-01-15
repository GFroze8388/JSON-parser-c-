let q1 = 0
let q2 = 0
let q3 = 0
let q4 = 0
let rightAnswers = 0
const answers = ['колотые', 'резаные', 'ушибленные', 'рубленные', 'рваные', 'укушенные', 'огнестрельные']
let truth
function a1(){
    q1 = 1
}
function a2(){
    q1 = 2
}
function a3(){
    q1 = 3
}
function a4(){
    q2 = 1
}
function a5(){
    q2 = 2
}
function a6(){
    q2 = 3
}
function a7(){
    q3 = 1
}
function a8(){
    q3 = 2
}
function a9(){
    q3 = 3
}
function a10(){
    q4 = 1
}
function a11(){
    q4 = 2
}
function a12(){
    q4 = 3
}
function check(){
    if (q1 != 3 && q1 != 0){
        let obj = document.querySelector(`#a${q1}`)
        obj.style.backgroundColor = "red"
    }else if (q1 == 3){
        rightAnswers++
        let obj = document.querySelector(`#a${q1}`)
        obj.style.backgroundColor = "lightgreen"
    }
    if (q2 != 1 && q2 != 0){
        let obj = document.querySelector(`#a${q2 + 3}`)
        obj.style.backgroundColor = "red"
    }else if (q2 == 1){
        rightAnswers++
        let obj = document.querySelector(`#a${q2 + 3}`)
        obj.style.backgroundColor = "lightgreen"
    }
    if (q3 != 1 && q3 != 0){
        let obj = document.querySelector(`#a${q3 + 6}`)
        obj.style.backgroundColor = "red"
    }else if (q3 == 1){
        rightAnswers++
        let obj = document.querySelector(`#a${q3 + 6}`)
        obj.style.backgroundColor = "lightgreen"
    }
    let input = document.getElementById('input').value
    for (let i = -1; i < 6; i++){
        if (input == answers[i]){
            truth = true
            rightAnswers++
            break
        }
    }
    if (truth == true){
        document.getElementById('input').style.color = "lightgreen"
    } else{
        document.getElementById('input').style.color = "red"
    }
    if (q4 != 3 && q4 != 0){
        let obj = document.querySelector(`#a${q4 + 9}`)
        obj.style.backgroundColor = "red"
    }else if (q4 == 3){
        rightAnswers++
        let obj = document.querySelector(`#a${q4 + 9}`)
        obj.style.backgroundColor = "lightgreen"
    }
    document.getElementById("rightAnswers").innerText = `Правильных ответов: ${rightAnswers}`
}