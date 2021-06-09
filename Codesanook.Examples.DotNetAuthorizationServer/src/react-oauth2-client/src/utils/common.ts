export function isEmpty(e: any) {
    switch (e) {
      case "":
      case 0:
      case "0":
      case null:
      case false:
      case undefined:
      case typeof (e) == "undefined":
        return true;
      default:
        return false;
    }
  }
  